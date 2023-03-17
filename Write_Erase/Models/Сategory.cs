using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Сategory
{
    public int Idсategories { get; set; }

    public string? СategoryProduct { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
