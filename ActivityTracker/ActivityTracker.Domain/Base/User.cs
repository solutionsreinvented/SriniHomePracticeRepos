using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

namespace ActivityTracker.Domain.Base
{
    public class User : PropertyStore, IUser
    {
        public User()
        {

        }

        public int Id { get => Get<int>(); set => Set(value); }

        public string FullName { get => Get<string>(); set => Set(value); }

        public string Password { get => Get<string>(); set => Set(value); }

        public UserRole UserRole { get => Get(UserRole.Standard); set => Set(value); }
    }
}
