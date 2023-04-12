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
using Write_Erase.Views;

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

        public string Title { get; set; } = ChangeableProduct.AddOrChange == true ? "Добавление продукта": "Изменение продукта";
        public string TextButton { get; set; } = ChangeableProduct.AddOrChange == true ? "Добавить": "Изменить";

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

        public List<string> Measurement { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Manufacturer { get; set; } = new List<string>(); 
        public List<string> Supplier { get; set; } = new List<string>();

        public List<Product> Products { get; set; } = new List<Product> ();

        public List<Measurement> MeasurementDb { get; set; } = new List<Measurement>();
        public List<Сategory> CategoriesDb { get; set; } = new List<Сategory>();
        public List<Manufacturer> ManufacturerDb { get; set; } = new List<Manufacturer>(); 
        public List<Supplier> SupplierDb { get; set; } = new List<Supplier>(); 
        
        public mChangeViewModel(ProductService productService, PageService pageService)
        {
            _pageService= pageService;
            _productService= productService;
            LoadData();
            
        }

        public DelegateCommand AddProduct => new(() =>
        {
            Article = Products.Where(i => i.ProductArticleNumber.Equals(Article)).SingleOrDefault().ProductArticleNumber;

            if (SelectedSupplier != null && SelectedMeasurement != null && SelectedCategory != null && SelectedManufacturer != null)
            {
                Product product = new Product
                {
                    ProductArticleNumber = Article,
                    ProductName = Name,
                    ProductDescription = Description,
                    ProductQuantityInStock = Convert.ToInt32(Count),
                    ProductCost = Convert.ToDecimal(Cost),
                    ProductDiscountAmount = Convert.ToInt32(Discount),
                    ProducMaxDiscount = Convert.ToInt32(MaxDiscount),
                    //ProductPhoto = "picture.png",
                    ProductMeasurement = MeasurementDb.Where(i => i.Measurement1.Equals(SelectedMeasurement)).SingleOrDefault().Idmeasurements,
                    ProductCategory = CategoriesDb.Where(i => i.СategoryProduct.Equals(SelectedCategory)).SingleOrDefault().Idсategories,
                    ProductManufacturer = ManufacturerDb.Where(i => i.Manufacturer1.Equals(SelectedManufacturer)).SingleOrDefault().Idmanufacturers,
                    ProductSupplier = SupplierDb.Where(i => i.Supplier1.Equals(SelectedSupplier)).SingleOrDefault().Idsuppliers,
                };
                _productService.AddProduct(product);
            }
        });

        public DelegateCommand GetOut => new(() => _pageService.ChangePage(new ViewItems()));

        public async void LoadData()
        {
            var measurements = await _productService.GetMeasurements();
            var products = await _productService.GetProducts();
            var categories = await _productService.GetСategories();
            var manufacturers = await _productService.GetManufacturers();
            var suppliers = await _productService.GetSuppliers();

            foreach( var product in products ) Products.Add( product );
            foreach (var item in measurements)
            {
                Measurement.Add(item.Measurement1); 
                MeasurementDb.Add(item); 
            }
            foreach (var item in categories)
            {
                Categories.Add(item.СategoryProduct);
                CategoriesDb.Add(item);
            }
            foreach (var item in manufacturers)
            {
                Manufacturer.Add(item.Manufacturer1);
                ManufacturerDb.Add(item);
            }
            foreach (var item in suppliers)
            {
                Supplier.Add(item.Supplier1);
                SupplierDb.Add(item);
            }


            if (!ChangeableProduct.AddOrChange && MeasurementDb.Count != 0)
            {
                Article = ChangeableProduct.Product.ProductArticleNumber;
                Name = ChangeableProduct.Product.ProductName;
                Description = ChangeableProduct.Product.ProductDescription;
                Count = ChangeableProduct.Product.ProductQuantityInStock.ToString();
                Cost = ChangeableProduct.Product.ProductCost.ToString();
                Discount = ChangeableProduct.Product.ProductDiscountAmount.ToString();
                MaxDiscount = ChangeableProduct.Product.ProducMaxDiscount.ToString();
                SelectedMeasurement = MeasurementDb.Where(i => i.Idmeasurements == ChangeableProduct.Product.ProductMeasurement).SingleOrDefault().Measurement1;
                SelectedCategory = CategoriesDb.Where(i => i.Idсategories == ChangeableProduct.Product.ProductCategory).SingleOrDefault().СategoryProduct;
                SelectedManufacturer = ManufacturerDb.Where(i => i.Idmanufacturers == ChangeableProduct.Product.ProductManufacturer).SingleOrDefault().Manufacturer1;
                SelectedSupplier = SupplierDb.Where(i => i.Idsuppliers == ChangeableProduct.Product.ProductSupplier).SingleOrDefault().Supplier1;

            }
        }


        
    }
}
