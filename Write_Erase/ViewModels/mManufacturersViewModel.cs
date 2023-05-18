using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Write_Erase.Models;
using Write_Erase.Services;
using Write_Erase.Views;

namespace Write_Erase.ViewModels
{
    public class mManufacturersViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Manufacturer> Manufacturers { get; set; }

        public string NameManufacturer { get; set; }

        public mManufacturersViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            GetManufacturers();
        } 
        public async void GetManufacturers()
        {
            var manufacturers = await _productService.GetManufacturers();
            Manufacturers = manufacturers;
        }

        public DelegateCommand AddManufacturer => new(() =>
        {
            if (NameManufacturer != null && NameManufacturer.Length != 0)
                _productService.AddManufacterer(NameManufacturer);
        });
    }
}
