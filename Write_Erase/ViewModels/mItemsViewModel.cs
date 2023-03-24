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
using Write_Erase.Models;
using Write_Erase.Services;
using Write_Erase.Views;

namespace Write_Erase.ViewModels
{
    public class mItemsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public Product SelectedProduct { get; set; }
        public Visibility VisibleButton { get; set; } = Visibility.Hidden;
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
        }

        public mItemsViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
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
                VisibleButton = Visibility.Visible;
                ProductCard product = ProductsInBasket.Products.Where(i => i.Product.ProductArticleNumber == SelectedProduct.ProductArticleNumber).FirstOrDefault();
                if (product != null)
                {
                    product.Count += 1;
                }
                else
                {
                    ProductCard productCard = new ProductCard();
                    productCard.Product = SelectedProduct;
                    productCard.Count += 1;
                    ProductsInBasket.Products.Add(productCard);
                }
            }
        });

        public DelegateCommand SignInBasket=> new(() => _pageService.ChangePage(new BasketPage()));
    }
}
