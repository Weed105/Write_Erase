using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Write_Erase.Models;

namespace Write_Erase.Converters
{

    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Length<3)
            {
                if ((value as int?) == 0 || value == null) return default;
                else return TextDecorations.Strikethrough;
            }
            else
            {
                List<Product> orders = GlobalOrderProducts.Products;
                if (orders.Where(i => i.ProductArticleNumber == value.ToString()).SingleOrDefault().ProductDiscountAmount != 0)
                    return orders.Where(i => i.ProductArticleNumber == value.ToString()).SingleOrDefault().ProductCost - (orders.Where(i => i.ProductArticleNumber == value.ToString()).SingleOrDefault().ProductCost * orders.Where(i => i.ProductArticleNumber == value.ToString()).SingleOrDefault().ProductDiscountAmount / 100);
                else
                    return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
