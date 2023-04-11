using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
{
    public class AdminUser : User, IUser
    {
        public AdminUser()
        {
            UserRole = UserRole.Admin;
        }
    }
}
