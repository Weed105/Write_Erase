using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Write_Erase.Models;
using Write_Erase.Services;

namespace Write_Erase.ViewModels
{
    public class mBasketViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderProductService _orderProductService;

        public string NotDiscountPrice { get; set; }
        public string Discount { get; set; }
        public string DiscountPrice { get; set; }
        public string Count { get; set; }
        public List<string> Points { get; set; } = new List<string>();
        public List<PickupPoint> Points_ID { get; set; } = new List<PickupPoint>();

        public ProductCard SelectedItem { get; set; }

        public List<ProductCard> Products { get; set; }

        public List<Orderproduct> Orderproducts { get; set; }


        public string SelectedPoint { get; set; }

        public mBasketViewModel(PageService pageService, ProductService productService, OrderProductService orderProductService)
        {
            _pageService = pageService;
            _productService = productService;
            _orderProductService = orderProductService;
            Products = ProductsInBasket.Products.ToList();
            PointUpdate();
            UpdateSum();
        }

        public DelegateCommand IncreaseNumber => new(() =>
        {
            
            if (SelectedItem != null)
            {
                ProductCard? selectedProductCard = ProductsInBasket.Products.Where(i => i == SelectedItem).FirstOrDefault();
                if (selectedProductCard != null && selectedProductCard.Count != selectedProductCard.Product.ProductQuantityInStock)
                {
                    selectedProductCard.Count += 1;                    
                }
            }
            UpdateSum();
        });

        public DelegateCommand ReduceNumber => new(() =>
        {
            if (SelectedPoint != null)
            {
                ProductCard? selectedProductCard = ProductsInBasket.Products.Where(i => i == SelectedItem).FirstOrDefault();
                if (selectedProductCard != null)
                {
                    selectedProductCard.Count -= 1;                    
                    if (selectedProductCard.Count == 0)
                    {
                        ProductsInBasket.Products.Remove(selectedProductCard);
                        Products = ProductsInBasket.Products.ToList();
                    }
                }
            }
            UpdateSum();

        });

        public DelegateCommand OrderCard => new(() =>
        {
            if (SelectedPoint != null)
            {
                string Connect = "Database=trade;Data Source=localhost;User Id=root;Password=1234";
                MySqlConnection myConnection = new MySqlConnection(Connect);
                int id_pickup = 0;
                foreach(PickupPoint pickupPoint in Points_ID)
                {
                    if (pickupPoint.Index.ToString().Equals( SelectedPoint.Split(", ", StringSplitOptions.RemoveEmptyEntries)[0]))
                    {
                        id_pickup = pickupPoint.IdpickupPoint;
                    }
                }
                _orderProductService.AddOrder(id_pickup, ProductsInBasket.Products);
            }
        });



        public void UpdateSum()
        {
            decimal sum_not_disc = 0;
            decimal? discount = 0;
            decimal? discount_price = 0;
            int count  = 0;
            
            foreach (var product in ProductsInBasket.Products)
            {
                sum_not_disc += product.Product.ProductCost * product.Count;
                discount_price += (product.Product.ProductCost - (product.Product.ProductCost * product.Product.ProductDiscountAmount / 100)) * product.Count;
                discount = (sum_not_disc - discount_price) / (sum_not_disc / 100);
                count += product.Count;
            }
            sum_not_disc = Math.Round(sum_not_disc, 2);
            discount_price = Math.Round((Decimal)discount_price, 2);
            discount = Math.Round((Decimal)discount, 2);

            NotDiscountPrice = sum_not_disc.ToString();
            Discount = discount.ToString();
            DiscountPrice = discount_price.ToString();
            Count = count.ToString();
        }
        
        public async void PointUpdate()
        {
            var points = await _productService.GetPoints();
            foreach (var point in points)
            {
                Points.Add(point.Index + ", " + point.City + ", " + point.Street + ", " + point.House);
                Points_ID.Add(point);
            }
        }

    }
}
