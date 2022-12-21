using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public class AdminUser : User, IUser
    {
        public AdminUser()
        {
            UserRole = UserRole.Admin;
        }
    }
}
