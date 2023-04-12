using System;
using System.Collections.Generic;

namespace Write_Erase.Models;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int ProductCategory { get; set; }

    public string ProductPhoto { get; set; } = null!;

    public int ProductManufacturer { get; set; }

    public decimal ProductCost { get; set; }

    public int? ProductDiscountAmount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public int ProducMaxDiscount { get; set; }

    public int ProductMeasurement { get; set; }

    
    public int ProductSupplier { get; set; }


    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();

    public virtual Сategory ProductCategoryNavigation { get; set; } = null!;

    public virtual Manufacturer ProductManufacturerNavigation { get; set; } = null!;

    public virtual Measurement ProductMeasurementNavigation { get; set; } = null!;

    public virtual Supplier ProductSupplierNavigation { get; set; } = null!;
}
