using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Write_Erase.Models;
using Write_Erase.Services;
using Write_Erase.Views;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Write_Erase.ViewModels
{
    public class mOrderViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderProductService _orderProductService;
        public string ListWidth { get; set; } = "auto";
        public Visibility VisibleButton { get; set; } = Visibility.Collapsed;
        public List<Product> Products { get; set; } = new List<Product>();
        public string FullName { get; set; } = Global.CurrentUser == null || Global.CurrentUser.UserName == string.Empty ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";

        public OrderUser SelectedOrder { get; set; }
        public List<string> Sorts { get; set; } = new List<string>() { "По возрастанию", "По убыванию" };
        public List<string> Statuses { get; set; } = new List<string>() { "Завершен", "Новый" };
        public List<string> Filters { get; set; } = new List<string> { "Все диапазоны", "0-10%", "11-14%", "15% и более" };
        public string Sorting
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: Load); }
        }

        public string Filter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: Load); }
        }

        public List<OrderUser> Orders { get; set; }= new List<OrderUser>();

        public mOrderViewModel(PageService pageService, ProductService productService, OrderProductService orderProductService)
        {
            _pageService = pageService;
            _productService = productService;
            _orderProductService = orderProductService;
            Load();
        }
        public async void Load()
        {
            var orderProducts = await _orderProductService.GetOrdersUser();
            if (!string.IsNullOrEmpty(Sorting))
            {
                switch (Sorting)
                {
                    case "По возрастанию":
                        orderProducts = orderProducts.OrderBy(p => p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count))).ToList();
                        break;
                    case "По убыванию":
                        orderProducts = orderProducts.OrderByDescending(p => p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count))).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(Filter))
            {
                switch (Filter)
                {
                    case "0-10%":
                        orderProducts = orderProducts.Where(p => (p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) - p.Orderproducts.Sum(i => (i.ProductArticleNumberNavigation.ProductCost - (i.ProductArticleNumberNavigation.ProductCost * i.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(i.Count))) / p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) * 100 >= 0 && (p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) - p.Orderproducts.Sum(i => (i.ProductArticleNumberNavigation.ProductCost - (i.ProductArticleNumberNavigation.ProductCost * i.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(i.Count))) / p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) * 100 < 11).ToList();
                        break;
                    case "11-14%":
                        orderProducts = orderProducts.Where(p => (p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) - p.Orderproducts.Sum(i => (i.ProductArticleNumberNavigation.ProductCost - (i.ProductArticleNumberNavigation.ProductCost * i.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(i.Count))) / p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) * 100 >= 11 && (p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) - p.Orderproducts.Sum(i => (i.ProductArticleNumberNavigation.ProductCost - (i.ProductArticleNumberNavigation.ProductCost * i.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(i.Count))) / p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) * 100 < 15).ToList();
                        break;
                    case "15% и более":
                        orderProducts = orderProducts.Where(p => (p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) - p.Orderproducts.Sum(i => (i.ProductArticleNumberNavigation.ProductCost - (i.ProductArticleNumberNavigation.ProductCost * i.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(i.Count))) / p.Orderproducts.Sum(i => i.ProductArticleNumberNavigation.ProductCost * decimal.Parse(i.Count)) * 100 >= 15).ToList();
                        break;
                }
            }
            Orders = orderProducts;
            var orders = await _orderProductService.GetOrders();
            GlobalOrderProducts.Orderproduct = orders;
        }

        public DelegateCommand ViewProducts => new(() =>
        {
            if (SelectedOrder != null)
            {
                VisibleButton = Visibility.Visible;
                ListWidth = "";
                SelectProducts();
            }
        });

        public async void SelectProducts()
        {
            if (SelectedOrder != null)
            {
                var updateProducts = await _productService.GetProducts();
                List<Orderproduct> orderProducts = (List<Orderproduct>)Orders.Where(i => i.Equals(SelectedOrder)).SingleOrDefault().Orderproducts;
                List<Product> products = new List<Product>();
                foreach (Orderproduct orderproduct in orderProducts)
                {
                    products.Add(updateProducts.Where(i => i.ProductArticleNumber == orderproduct.ProductArticleNumber).SingleOrDefault());
                }
                Products = products;
            }
        }

        public DelegateCommand MinProducts => new(() =>
        {
            VisibleButton = Visibility.Collapsed;
            ListWidth = "auto";
            Products = new List<Product>();
        });

        public DelegateCommand ChangeDate => new(() =>
        {
            _orderProductService.ChangeOrder(SelectedOrder);
        });

        public DelegateCommand ChangeStatus => new(() =>
        {
            if (SelectedOrder.OrderStatus == 1)
                SelectedOrder.OrderStatus = 2;
            else
                SelectedOrder.OrderStatus = 1;
            _orderProductService.ChangeState(SelectedOrder);
            Load();
        });

        public DelegateCommand SignInProducts => new(() => _pageService.ChangePage(new ViewItems()));
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new HomePage());
        });
    }
}
