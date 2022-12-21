using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Base
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
