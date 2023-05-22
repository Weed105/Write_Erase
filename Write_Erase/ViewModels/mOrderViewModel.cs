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
using static iTextSharp.text.pdf.AcroFields;
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
        public List<string> Filters { get; set; } = new List<string> { "Все диапазоны", "Завершен", "Новый"};
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
                    case "Завершен":
                        orderProducts = orderProducts.Where(p => p.OrderStatusNavigation.OrderStatus1 == "Завершен").ToList();
                        break;
                    case "Новый":
                        orderProducts = orderProducts.Where(p => p.OrderStatusNavigation.OrderStatus1 == "Новый ").ToList();
                        break; 
                }
            }
            Orders = orderProducts;
            foreach (var o in Orders)
            {
                foreach(var p in o.Orderproducts)
                {
                    if (!p.ProductArticleNumberNavigation.ProductPhoto.StartsWith("C"))
                        p.ProductArticleNumberNavigation.ProductPhoto = p.ProductArticleNumberNavigation.ProductPhoto == string.Empty ? System.IO.Path.GetFullPath("../../../Resources/picture.png") : System.IO.Path.GetFullPath("../../../Resources/" + p.ProductArticleNumberNavigation.ProductPhoto);
                }
            }
            var orders = await _orderProductService.GetOrders();
            GlobalOrderProducts.Orderproduct = orders;
        }
         

        public DelegateCommand ChangeDate => new(() =>
        {
            _orderProductService.ChangeOrder(SelectedOrder);
        });

        public DelegateCommand ChangeStatus => new(() =>
        {
            if (SelectedOrder.OrderStatus == 2)
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
