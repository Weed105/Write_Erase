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
    public class mSuppliersViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Supplier> Suppliers { get; set; }
        public Supplier SelectedSupplier{ get; set; }

        public string NameSupplier { get; set; }

        public mSuppliersViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            GetSuppliers();
        }
        public async void GetSuppliers()
        {
            var suppliers = await _productService.GetSuppliers();
            Suppliers = suppliers;
        }
        public DelegateCommand AddSupplier => new(() =>
        {
            if (NameSupplier != null && NameSupplier != "")
                _productService.AddSupplier(NameSupplier);
            GetSuppliers();
        });

        public DelegateCommand ChangeSupplier => new(() =>
        {
            if (SelectedSupplier != null && NameSupplier != null && NameSupplier != "")
            {
                _productService.ChangeSupplier(SelectedSupplier, NameSupplier);
            }
            else
            {
                MessageBox.Show("Для изменения поставщика выберите необходимого из списка и введите в строку имя поставщика", "Внимание");
            }
            GetSuppliers();
        });

        public DelegateCommand SignInProducts => new(() => _pageService.ChangePage(new ViewItems()));
    }
}
