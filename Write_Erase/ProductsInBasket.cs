using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;

namespace Write_Erase
{
    static class ProductsInBasket
    {
        public static ObservableCollection<ProductCard> Products { get; set; } = new ObservableCollection<ProductCard>();
    }
}
