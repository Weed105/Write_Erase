using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Write_Erase.Models;

namespace Write_Erase.Converters
{
    public class OrderPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Orderproduct> orders = GlobalOrderProducts.Orderproduct;
            decimal? discount_price = 0;
            foreach (Orderproduct item in orders)
            {
                if (item.OrderId == (int)value)
                {
                    discount_price += (item.ProductArticleNumberNavigation.ProductCost - (item.ProductArticleNumberNavigation.ProductCost * item.ProductArticleNumberNavigation.ProductDiscountAmount / 100)) * decimal.Parse(item.Count);
                }
            }
            discount_price = Math.Round((Decimal)discount_price, 2);
            return discount_price;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
