using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
{
    public class StandardUser : User, IUser
    {
        public StandardUser()
        {
            UserRole = UserRole.Standard;
        }
    }
}
