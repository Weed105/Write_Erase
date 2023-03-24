using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Write_Erase.Models;
using Write_Erase.Services;

namespace Write_Erase.ViewModels
{
    public class mBasketViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public ProductCard SelectedItem { get; set; }

        public List<ProductCard> Products { get; set; }

        public mBasketViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Products = ProductsInBasket.Products.ToList();
        }

        public DelegateCommand IncreaseNumber => new(() =>
        {
            if (SelectedItem != null)
            {
                ProductCard selectedProductCard = ProductsInBasket.Products.Where(i => i == SelectedItem).FirstOrDefault();
                if (selectedProductCard != null && selectedProductCard.Count != selectedProductCard.Product.ProductQuantityInStock)
                {
                    selectedProductCard.Count += 1;                    
                }
            }
        });

        public DelegateCommand ReduceNumber => new(() =>
        {
            if (SelectedItem != null)
            {
                ProductCard selectedProductCard = ProductsInBasket.Products.Where(i => i == SelectedItem).FirstOrDefault();
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
        });
    }
}
