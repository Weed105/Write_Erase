using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Manufacturer
{
    public int Idmanufacturers { get; set; }

    public string? Manufacturer1 { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
