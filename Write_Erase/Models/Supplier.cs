using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Supplier
{
    public int Idsuppliers { get; set; }

    public string? Supplier1 { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
