using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Write_Erase.Models;
using Write_Erase.Services;

namespace Write_Erase.ViewModels
{
    public class mChangeViewModel : BindableBase, IDataErrorInfo
    {
        Dictionary<string, string> errors = new Dictionary<string, string>();
        private string _name;
        private string _article;

        public string Error => throw new NotImplementedException();
        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;
        public bool IsValid => errors.Values.Any(x => x == null);


        private readonly ProductService _productService;
        private readonly PageService _pageService;

        public string Name { get; set; }
        public string Article { get; set; }
        public string Description { get; set; }
        public string Count { get; set; }
        public string Cost { get; set; }
        public string Discount { get; set; }
        public string MaxDiscount { get; set; }
        public string Image { get; set; }

        public string SelectedMeasurement { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedManufacturer { get; set; }
        public string SelectedSupplier { get; set; }
        public string SelectedStatus { get; set; }

        public List<string> Measurement { get; set; } = new List<string>();
        public List<string> Category { get; set; }= new List<string>();
        public List<string> Manufacturer { get; set; } = new List<string>(); 
        public List<string> Supplier { get; set; } = new List<string>(); 
        
        public mChangeViewModel(ProductService productService, PageService pageService)
        {
            _pageService= pageService;
            _productService= productService;
            LoadData();
        }

        public DelegateCommand AddProduct => new(() =>
        {


            Product product = new Product
            {
                ProductArticleNumber = Article,
                ProductName = Name,
                ProductDescription= Description,
                ProductQuantityInStock= Convert.ToInt32(Count),
                ProductCost = Convert.ToInt32(Cost),
                ProductDiscountAmount= Convert.ToInt32(Discount),
                ProducMaxDiscount= Convert.ToInt32(MaxDiscount),
                ProductPhoto = Image,

                
            };
        });

        public async void LoadData()
        {
            var measurements = await _productService.GetMeasurements();
            var categories = await _productService.GetСategories();
            var manufacturers = await _productService.GetManufacturers();
            var suppliers = await _productService.GetSuppliers();

            foreach (var item in measurements) Measurement.Add(item.Measurement1);
            foreach (var item in categories) Category.Add(item.СategoryProduct);
            foreach (var item in manufacturers) Manufacturer.Add(item.Manufacturer1);
            foreach (var item in suppliers) Supplier.Add(item.Supplier1);
        }

        
    }
}
