using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                await _context.Manufacturers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        products.Add(new Product
                        {
                            ProductPhoto = item.ProductPhoto == string.Empty ? "../../../Resources/picture.png" : System.IO.Path.GetFullPath("../../../Resources/" + item.ProductPhoto),
                            ProductName = item.ProductName,
                            ProductDescription= item.ProductDescription,
                            ProductManufacturer= item.ProductManufacturer,
                            ProducMaxDiscount = item.ProducMaxDiscount,
                            ProductCategory= item.ProductCategory,
                            ProductMeasurement= item.ProductMeasurement,
                            ProductSupplier = item.ProductSupplier,
                            ProductManufacturerNavigation = item.ProductManufacturerNavigation,
                            ProductCategoryNavigation = item.ProductCategoryNavigation,
                            ProductMeasurementNavigation= item.ProductMeasurementNavigation,
                            ProductSupplierNavigation= item.ProductSupplierNavigation,
                            ProductCost = item.ProductCost,
                            ProductDiscountAmount= item.ProductDiscountAmount.Value,
                            ProductArticleNumber= item.ProductArticleNumber,
                            ProductQuantityInStock= item.ProductQuantityInStock,
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
        
        public async void AddProduct(Product product)
        {
            //var products = _context.Products.ToListAsync();
            //var getProducts = await GetProducts();
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}
