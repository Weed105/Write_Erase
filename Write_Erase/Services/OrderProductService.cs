using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
