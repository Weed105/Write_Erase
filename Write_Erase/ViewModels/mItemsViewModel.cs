using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;
using Write_Erase.Models;
using Write_Erase.Services;
using Write_Erase.Views;

namespace Write_Erase.ViewModels
{
    public class mItemsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderProductService _orderProductService;

        public Product SelectedProduct { get; set; }
        public Visibility VisibleButton { get; set; } = Visibility.Hidden;
        public Visibility VisibleMenu{ get; set; } = Global.CurrentUser != null && Global.CurrentUser.UserRole == 1 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VisibleOrder { get; set; } = Global.CurrentUser != null && (Global.CurrentUser.UserRole == 1 || Global.CurrentUser.UserRole == 3) ? Visibility.Visible : Visibility.Hidden;

        public string FullName { get; set; } = Global.CurrentUser == null || Global.CurrentUser.UserName == string.Empty ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";

        public List<string> Sorts { get; set; } = new List<string>() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new List<string> { "Все диапазоны", "0-5%", "5-9%", "9% и более" };

        public int AllCountItems { get; set; } = 0;
        public int CountItems { get; set; } = 0;
        public List<Product> Products { get; set; }

        public string Search
        {
            get { return GetValue<string>();  }
            set { SetValue(value, changedCallback: Update); }
        }
        public string Sorting
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: Update);}
        }
        public string Filter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: Update);}
        }


        public async void Update()
        {
            var updateProducts = await _productService.GetProducts();
            AllCountItems= updateProducts.Count;
            if (!string.IsNullOrEmpty(Search))
            {
                updateProducts = updateProducts.Where(p => p.ProductName.ToLower().Contains(Search.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Sorting))
            {
                switch(Sorting)
                {
                    case "По возрастанию":
                        updateProducts = updateProducts.OrderBy(p => p.ProductCost).ToList();
                        break;
                    case "По убыванию":
                        updateProducts = updateProducts.OrderByDescending(p => p.ProductCost).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(Filter))
            {
                switch (Filter)
                {
                    case "0-5%":
                        updateProducts = updateProducts.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 5).ToList();
                        break;
                    case "5-9%":
                        updateProducts = updateProducts.Where(p => p.ProductDiscountAmount >= 5 && p.ProductDiscountAmount < 9).ToList();
                        break;
                    case "9% и более":
                        updateProducts = updateProducts.Where(p => p.ProductDiscountAmount >= 9).ToList();
                        break;
                }
            }
            CountItems = updateProducts.Count;
            Products = updateProducts;

            if (ProductsInBasket.Products.Count != 0)
                VisibleButton = Visibility.Visible;
            else
                VisibleButton = Visibility.Hidden;

            GlobalOrderProducts.Products = Products;

            var orders = await _orderProductService.GetOrders();
            GlobalOrderProducts.Orderproduct = orders;
        }

        public mItemsViewModel(PageService pageService, ProductService productService, OrderProductService orderProductService)
        {
            _pageService = pageService;
            _productService = productService;
            _orderProductService = orderProductService;
            Update();
        }
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new HomePage());
        });

        public DelegateCommand AddToBasket => new(() =>
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.ProductQuantityInStock != 0)
                {
                    VisibleButton = Visibility.Visible;
                    ProductCard product = ProductsInBasket.Products.Where(i => i.Product.ProductArticleNumber == SelectedProduct.ProductArticleNumber).FirstOrDefault();
                    if (product != null)
                    {
                        if (product.Count != product.Product.ProductQuantityInStock)
                            product.Count += 1;
                        else
                            MessageBox.Show("В заказе уже этого полно", "Хватит");
                    }
                    else
                    {
                        ProductCard productCard = new ProductCard();
                        productCard.Product = SelectedProduct;
                        productCard.Count += 1;
                        ProductsInBasket.Products.Add(productCard);
                    }
                }
                else
                    MessageBox.Show("На складе нет продукта", "Внимание");
            }
        });

        public DelegateCommand AddProduct => new(() =>
        {
            ChangeableProduct.Product = SelectedProduct;
            ChangeableProduct.AddOrChange = true;
            _pageService.ChangePage(new ChangePage());
        });
        
        public DelegateCommand ChangeProduct => new(() =>
        {
            ChangeableProduct.Product = SelectedProduct;
            ChangeableProduct.AddOrChange = false;
            _pageService.ChangePage(new ChangePage());
        });

        public DelegateCommand DeleteProduct => new(() =>
        {
            _productService.DeleteProduct(SelectedProduct);
            Update();
        });
        public DelegateCommand SignInBasket=> new(() => _pageService.ChangePage(new BasketPage()));
        public DelegateCommand SignInOrders => new(() => _pageService.ChangePage(new OrderPage()));

        public DelegateCommand ViewManufacturers => new(() => _pageService.ChangePage(new ManufacturersPage()));
        public DelegateCommand ViewSuppliers => new(() => _pageService.ChangePage(new SuppliersPage()));
    }
}
