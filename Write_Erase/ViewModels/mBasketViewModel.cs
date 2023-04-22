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
using Write_Erase.Views;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;

namespace Write_Erase.ViewModels
{
    public class mBasketViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderProductService _orderProductService;
        public string FullName { get; set; } = Global.CurrentUser == null || Global.CurrentUser.UserName == string.Empty ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";

        public string NotDiscountPrice { get; set; }
        public string Discount { get; set; }
        public string DiscountPrice { get; set; }
        public string Count { get; set; }
        public List<string> Points { get; set; } = new List<string>();
        public List<PickupPoint> Points_ID { get; set; } = new List<PickupPoint>();
        public List<OrderUser> OrderUsers { get; set; } = new List<OrderUser>();

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
            if (SelectedItem != null)
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
                foreach (PickupPoint pickupPoint in Points_ID)
                {
                    if (pickupPoint.Index.ToString().Equals(SelectedPoint.Split(", ", StringSplitOptions.RemoveEmptyEntries)[0]))
                    {
                        id_pickup = pickupPoint.IdpickupPoint;
                    }
                }
                _orderProductService.AddOrder(id_pickup, ProductsInBasket.Products);
                CreateCoupon();

            }
        });

        public void CreateCoupon()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("coupon.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            Font fontBold = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            fontBold.SetStyle("bold");

            Paragraph paragraph = new Paragraph("Это ваш заказ", font);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            Paragraph numberOrder = new Paragraph($"Номер заказа: {OrderUsers.Count}", font);
            doc.Add(numberOrder);

            Paragraph dateOrder = new Paragraph($"Дата заказа: {DateTime.Now.ToString("D")}", font);
            doc.Add(dateOrder);

            Paragraph structureOrder = new Paragraph($"Состав заказа:", font);
            doc.Add(structureOrder);

            foreach(ProductCard product in ProductsInBasket.Products)
            {
                Paragraph itemOrder = new Paragraph($"- {product.Product.ProductName} {product.Count} {product.Product.ProductMeasurementNavigation.Measurement1};", font);
                doc.Add(itemOrder);
            }
            Paragraph sumOrder = new Paragraph($"Цена заказа без скидки: {NotDiscountPrice}₽", font);
            doc.Add(sumOrder);

            Paragraph discOrder = new Paragraph($"Скидка: {Discount}%", font);
            doc.Add(discOrder);

            Paragraph priceOrder = new Paragraph($"Цена заказа с учетом скидки: {DiscountPrice}₽", font);
            doc.Add(priceOrder);

            Paragraph punctOrder = new Paragraph($"Пункт выдачи заказа: {SelectedPoint}", font);
            doc.Add(punctOrder);

            Random random = new Random();
            Paragraph codeOrder = new Paragraph($"Код получения заказа: {(char)random.Next(48, 57)}{(char)random.Next(48, 57)}{(char)random.Next(48, 57)}", fontBold);
            doc.Add(codeOrder);

            bool date = false;
            foreach(ProductCard product in ProductsInBasket.Products)
            {
                if (product.Product.ProductQuantityInStock >= 3)
                    date = true;
                else
                {
                    date = false;
                    break;
                }
            }

            if (date)
            {
                Paragraph date2Order = new Paragraph($"Дата доставки заказа: {DateTime.Now.AddDays(3)}", font);
                doc.Add(date2Order);
            }
            else
            {
                Paragraph date2Order = new Paragraph($"Дата доставки заказа: {DateTime.Now.AddDays(6)}", font);
                doc.Add(date2Order);
            }

            doc.Close();
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("coupon.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }

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
            var orders = await _orderProductService.GetOrdersUser();
            OrderUsers = orders;
        }
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new HomePage());
        });
        public DelegateCommand SignInProducts => new(() => _pageService.ChangePage(new ViewItems()));

    }
}
