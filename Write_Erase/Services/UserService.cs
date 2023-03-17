using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;

namespace Write_Erase.Services
{
    public class UserService
    {
        public readonly TradeContext _context;
        public UserService(TradeContext context) 
        {
            _context = context;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                Global.CurrentUser = new User
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserSurname = user.UserSurname,
                    UserPatronymic = user.UserPatronymic,
                    UserRole = user.UserRole,
                };
                return true;
            }
            return false;
        }
    }
}
