using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Write_Erase.Models;

namespace Write_Erase.Converters
{
    public class OrderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Orderproduct> orders = GlobalOrderProducts.Orderproduct;
            int i = 0;
            foreach (Orderproduct item in orders)
                if (item.OrderId == (int)value)
                {
                    if (item.ProductArticleNumberNavigation.ProductQuantityInStock > 3)
                        i = 2;
                    if (item.ProductArticleNumberNavigation.ProductQuantityInStock == 0)
                    {
                        i = 0;
                        break;
                    }
                    if (item.ProductArticleNumberNavigation.ProductQuantityInStock <= 3)
                    {
                        i = 1;
                        break;
                    }
                    
                }
            if (i == 2)  return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#20b2aa"));
            if (i == 0) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff8c00"));
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
