using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Write_Erase.Models;
using Write_Erase.Services;

namespace Write_Erase.ViewModels
{
    public class mOrderViewModel
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderProductService _orderProductService;

        public List<OrderUser> Orders { get; set; }

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
            Orders = orderProducts;
        }
    }
}
