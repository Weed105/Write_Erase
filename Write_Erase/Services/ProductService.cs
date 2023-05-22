using DevExpress.Mvvm.Native;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Write_Erase.Models;

namespace Write_Erase.Services
{
    public class ProductService
    {
        public readonly TradeContext _context;

        public ProductService(TradeContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();

            try
            {
                var product = await _context.Products.ToListAsync();
                await _context.Measurements.ToListAsync();
                await _context.Manufacturers.ToListAsync();
                await _context.Orderproducts.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        //if (item.ProductStatus == 0)
                        //{
                        //    item.ProductPhoto = item.ProductPhoto == string.Empty ? "../../../Resources/picture.png" : System.IO.Path.GetFullPath("../../../Resources/" + item.ProductPhoto);
                        //    products.Add(item);
                        //}
                            products.Add(new Product
                            {
                                ProductPhoto = !item.ProductPhoto.StartsWith("C") ? item.ProductPhoto == string.Empty ? "../../../Resources/picture.png" : System.IO.Path.GetFullPath("../../../Resources/" + item.ProductPhoto) : item.ProductPhoto,
                                ProductName = item.ProductName,
                                ProductDescription = item.ProductDescription,
                                ProductManufacturer = item.ProductManufacturer,
                                ProducMaxDiscount = item.ProducMaxDiscount,
                                ProductCategory = item.ProductCategory,
                                ProductMeasurement = item.ProductMeasurement,
                                ProductSupplier = item.ProductSupplier,
                                ProductManufacturerNavigation = item.ProductManufacturerNavigation,
                                ProductCategoryNavigation = item.ProductCategoryNavigation,
                                ProductMeasurementNavigation = item.ProductMeasurementNavigation,
                                ProductSupplierNavigation = item.ProductSupplierNavigation,
                                ProductCost = item.ProductCost,
                                ProductDiscountAmount = item.ProductDiscountAmount.Value,
                                ProductArticleNumber = item.ProductArticleNumber,
                                ProductQuantityInStock = item.ProductQuantityInStock,

                            });
                    }
                });
            }
            catch { }
            
            return products;
        }

        public async Task<List<PickupPoint>> GetPoints()
        {
            List<PickupPoint> points = new();
            try
            {
                var puncts = await _context.PickupPoints.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var point in puncts)
                    {
                        points.Add(point);
                    }
                });
            }
            catch { }

            return points;
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            List<Measurement> measurements = new();
            try
            {
                var measurementsDb = await _context.Measurements.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var measurement in measurementsDb)
                    {
                        measurements.Add(measurement);
                    }
                });
            }
            catch { }
            return measurements;
        }

        public async Task<List<Сategory>> GetСategories()
        {
            List<Сategory> сategories = new();
            try
            {
                var сategoriesDb = await _context.Сategories.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var category in сategoriesDb)
                    {
                        сategories.Add(category);
                    }
                });
            }
            catch { }
            return сategories;
        }

        public async Task<List<Manufacturer>> GetManufacturers()
        {
            List<Manufacturer> manufacturers = new();
            try
            {
                var manufacturersDb = await _context.Manufacturers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var manufacturer in manufacturersDb)
                    {
                        manufacturers.Add(manufacturer);
                    }
                });
            }
            catch { }
            return manufacturers;
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            List<Supplier> suppliers = new();
            try
            {
                var suppliersDb = await _context.Suppliers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var supplier in suppliersDb)
                    {
                        suppliers.Add(supplier);
                    }
                });
            }
            catch { }
            return suppliers;
        }
        public async void AddManufacterer(string manufacturer)
        {
            int count_orders = _context.Manufacturers.Max(i => i.Idmanufacturers) + 1;
            if (_context.Manufacturers.Where(i => i.Manufacturer1 == manufacturer).Count() == 0)
            {

                _context.Manufacturers.Add(new Manufacturer
                {
                    Idmanufacturers = count_orders,
                    Manufacturer1 = manufacturer,
                });
                _context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Такой производитель уже есть!", "Внимание");
            }
        }

        public async void AddSupplier(string supplier)
        {
            int count_orders = _context.Suppliers.Max(i => i.Idsuppliers) + 1;
            if (_context.Suppliers.Where(i => i.Supplier1 == supplier).Count() == 0)
            {

                _context.Suppliers.Add(new Supplier
                {
                    Idsuppliers = count_orders,
                    Supplier1 = supplier,
                });
                _context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Такой поставщик уже есть!", "Внимание");
            }
        }

        public async void ChangeSupplier(Supplier supplier, string name_supplier)
        {

            if (_context.Suppliers.Where(i => i.Supplier1 == name_supplier).Count() == 0)
                _context.Suppliers.Where(i => i == supplier).SingleOrDefault().Supplier1 = name_supplier;
            else
                MessageBox.Show("Такой поставщик уже есть!", "Внимание");
            _context.SaveChanges();

        }

        public async void ChangeManufacterer(Manufacturer manufacturer, string name_manufacturer)
        {
            if (_context.Manufacturers.Where(i => i.Manufacturer1 == name_manufacturer).Count() == 0)
                _context.Manufacturers.Where(i => i == manufacturer).SingleOrDefault().Manufacturer1 = name_manufacturer;
            else
                MessageBox.Show("Такой производитель уже есть!", "Внимание");
            _context.SaveChanges();

        }

        public async void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public async void ChangeProduct(Product product)
        {
            Product product1 = _context.Products.Where(i => i.ProductArticleNumber == product.ProductArticleNumber).SingleOrDefault();
            product1.ProducMaxDiscount = product.ProducMaxDiscount;
            product1.ProductManufacturer = product.ProductManufacturer;
            product1.ProductCategory = product.ProductCategory;
            product1.ProductDescription = product.ProductDescription;
            product1.ProductPhoto = product.ProductPhoto;
            product1.ProductName = product.ProductName;
            product1.ProductMeasurement = product.ProductMeasurement;
            product1.ProductSupplier = product.ProductSupplier;
            product1.ProductCost = product.ProductCost;
            product1.ProductQuantityInStock = product.ProductQuantityInStock;
            product1.ProductDiscountAmount = product.ProductDiscountAmount;
            _context.SaveChanges();
        }

        public async void DeleteProduct(Product product)
        {
            _context.Products.Where(i => i.ProductArticleNumber == product.ProductArticleNumber).SingleOrDefault().ProductStatus = 1;
            _context.SaveChanges();
        }
    }
}
