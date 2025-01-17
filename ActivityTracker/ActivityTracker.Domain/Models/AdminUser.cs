using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public class AdminUser : User, IUser
    {
        public AdminUser()
        {
            UserRole = UserRole.Admin;
        }
    }
}
