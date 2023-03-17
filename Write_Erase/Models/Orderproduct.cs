using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Orderproduct
{
    public int OrderId { get; set; }

    public string ProductArticleNumber { get; set; } = null!;

    public string Count { get; set; } = null!;

    public virtual OrderUser Order { get; set; } = null!;

    public virtual Product ProductArticleNumberNavigation { get; set; } = null!;
}
