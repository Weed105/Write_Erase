using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;

namespace Write_Erase
{
    static class ChangeableProduct
    {
        public static Product Product { get; set; } = new Product();
        public static bool AddOrChange { get; set; }
    }
}
