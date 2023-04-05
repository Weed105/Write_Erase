using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Write_Erase.Models;

namespace Write_Erase.Services
{
    public class OrderProductService
    {
        public readonly TradeContext _context;

        public OrderProductService(TradeContext context)
        {
            _context = context;
        }

        public async Task<List<Orderproduct>> GetOrders()
        {
            List<Orderproduct> orders = new();
            try
            {
                var order = await _context.Orderproducts.ToListAsync();
                foreach (var orderproduct in order)
                {
                    orders.Add(orderproduct);
                }
            }
            catch { }
            return orders;
        }

        public async Task<List<OrderUser>> GetOrdersUser()
        {
            List<OrderUser> orderUsers = new();

            try
            {
                var orders = await _context.OrderUsers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var user in orders)
                    {
                        orderUsers.Add(user);
                    }
                });
            }
            catch { }

            return orderUsers;
        }

        public async void AddOrder(int pickup, ObservableCollection<ProductCard> values)
        {
            var orders = await GetOrders();
            var order_users = _context.OrderUsers.ToList();
            int count_orders = orders.Max(i => i.OrderId) + 1;
            _context.OrderUsers.Add(new OrderUser
            {
                OrderId = count_orders,
                FioClient = Global.CurrentUser.UserSurname + " " + Global.CurrentUser.UserName + " " + Global.CurrentUser.UserPatronymic,
                OrderDate = DateTime.Now,
                OrderDeliveryDate = DateTime.Now.AddYears(5),
                OrderStatus = 2,
                OrderPickupPoint = pickup,
                GetCode = order_users.Max(i => i.GetCode) + 1,
            });
            foreach (var item in values)
            {
                _context.Orderproducts.Add(new Orderproduct
                {
                    OrderId = count_orders,
                    ProductArticleNumber = item.Product.ProductArticleNumber,
                    Count = item.Count.ToString(),
                });
            }
            _context.SaveChanges();
        }


    }
}
