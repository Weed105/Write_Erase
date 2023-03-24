using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Erase.Models
{
    public  class ProductCard:BindableBase
    {
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
