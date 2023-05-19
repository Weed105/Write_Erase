using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Write_Erase.Models;

namespace Write_Erase.Converters
{
    internal class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Orderproduct> orders = GlobalOrderProducts.Orderproduct;
            decimal sum = 0;
            foreach (Orderproduct item in orders)
            {
                if (item.OrderId == (int)value)
                    sum += item.ProductArticleNumberNavigation.ProductCost * decimal.Parse(item.Count);
            }
            sum = Math.Round(sum, 2);
            return sum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
