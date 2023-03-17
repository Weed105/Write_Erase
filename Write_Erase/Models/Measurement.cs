using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Measurement
{
    public int Idmeasurements { get; set; }

    public string? Measurement1 { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
