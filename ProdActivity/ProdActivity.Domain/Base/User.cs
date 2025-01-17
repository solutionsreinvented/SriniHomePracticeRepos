using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Stores;

namespace ProdActivity.Domain.Base
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
