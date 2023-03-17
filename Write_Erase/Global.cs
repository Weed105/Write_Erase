using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;

namespace Write_Erase
{
    internal static class Global
    {
        public static User CurrentUser { get; set; } = new User
        {
            UserId = 0,
            UserName = string.Empty,
            UserSurname= string.Empty,
            UserPatronymic = string.Empty,
            UserRole = 0,
        };
    }
}
