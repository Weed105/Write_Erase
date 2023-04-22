using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;

namespace Write_Erase
{
    static class GlobalOrderProducts
    {
        public static List<Orderproduct> Orderproduct { get; set; } = new List<Orderproduct>();
        public static List<Product> Products { get; set; } = new List<Product>();
    }
}
