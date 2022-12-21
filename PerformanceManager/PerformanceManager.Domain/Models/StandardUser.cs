using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public class StandardUser : User, IUser
    {
        public StandardUser()
        {
            UserRole = UserRole.Standard;
        }
    }
}
