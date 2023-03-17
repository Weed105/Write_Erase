using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class PickupPoint
{
    public int IdpickupPoint { get; set; }

    public int Index { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public int? House { get; set; }

    public virtual ICollection<OrderUser> OrderUsers { get; } = new List<OrderUser>();
}
