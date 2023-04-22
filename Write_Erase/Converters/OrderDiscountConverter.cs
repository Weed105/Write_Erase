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
    public class OrderDiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Orderproduct> orders = GlobalOrderProducts.Orderproduct;
            decimal? discount_price = 0;
            decimal? sum = 0;
            foreach (Orderproduct item in orders)
            {
                if (item.OrderId == (int)value)
                {
                    sum += item.ProductArticleNumberNavigation.ProductCost * decimal.Parse(item.Count);
                    discount_price += (item.ProductArticleNumberNavigation.ProductCost - (item.ProductArticleNumberNavigation.ProductCost * item.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(item.Count);
                }
            }
            sum = (sum - discount_price) / sum * 100;
            sum = Math.Round((Decimal)sum, 2);
            return sum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
