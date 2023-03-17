using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class OrderStatus
{
    public int IdorderStatus { get; set; }

    public string OrderStatus1 { get; set; } = null!;

    public virtual ICollection<OrderUser> OrderUsers { get; } = new List<OrderUser>();
}
