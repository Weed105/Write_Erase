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
                            ProductManufacturerNavigation = item.ProductManufacturerNavigation,
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
    }
}
