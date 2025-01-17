using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public class StandardUser : User, IUser
    {
        public StandardUser()
        {
            UserRole = UserRole.Standard;
        }
    }
}
