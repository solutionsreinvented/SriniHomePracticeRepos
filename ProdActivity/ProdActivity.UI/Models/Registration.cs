using ProdActivity.Domain.Stores;

namespace ProdActivity.UI.Models
{
    public class Registration : PropertyStore
    {
        #region Default Constructor

        public Registration()
        {

        }

        #endregion

        #region Public Properties

        public string UserId { get => Get<string>(); set => Set(value); }

        public string Password { get => Get<string>(); set => Set(value); }

        public string LicenseFilePath { get => Get<string>(); set => Set(value); }

        public string RegistrationKey { get => Get<string>(); set => Set(value); }

        #endregion
    }
}
