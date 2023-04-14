using System.Windows.Input;

using ActivityTracker.Domain.Enums;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class LoginViewModel : ManageUserViewModel
    {
        #region Parameterized Constructor
        public LoginViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Initialize();
        }

        #endregion

        #region Public Properties
        public string UserId
        {
            get => Get<string>();
            set
            {
                Set(value);
                User = GetUser(value);
                Password = string.Empty;
                LoginAsAdmin = UserIsAdmin;
                RaiseMultiplePropertiesChanged(nameof(ValidUser), nameof(UserExists), nameof(UserIsAdmin));
            }
        }

        public string Password
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(ValidUser), nameof(UserExists));
            }
        }
        public bool LoginAsAdmin { get => Get<bool>(); set => Set(value); }

        //public bool IsLoggedIn { get => Get<bool>(); set => Set(value); }
        #endregion

        #region Readonly Properties
        public bool ValidUser => UserExists && ValidPassword();

        public bool UserIsAdmin => UserExists && User.UserRole == UserRole.Admin;

        #endregion

        #region Private Helpers
        private bool ValidPassword()
        {
            return User != null && User.Password == Password;

            ///return !string.IsNullOrEmpty(Password) && (Password.Length is >= 8 and <= 15) && Password.Any(c => char.IsDigit(c));
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand { get; set; }

        public ICommand LogInCommand { get; set; }
        #endregion

        #region Command Handlers
        private void OnLogIn()
        {
            if (ValidUser)
            {
                if (LoginAsAdmin)
                {
                    _navigationStore.DashboardViewModel = new AdminDashboardViewModel(_navigationStore) { };
                }
                else
                {
                    _navigationStore.DashboardViewModel = new StandardDashboardViewModel(_navigationStore) { };
                }

                IsLoggedIn = true;
            }
        }

        private void OnChangePassword()
        {
            _navigationStore.ManageUserViewModel = new ChangePasswordViewModel(_navigationStore)
            {
                UserId = UserId,
            };
        }
        #endregion

        #region Abstract Methods Implementation
        protected override void Initialize()
        {
            ChangePasswordCommand = new RelayCommand(OnChangePassword, true);
            LogInCommand = new RelayCommand(OnLogIn, true);
        }
        #endregion

    }
}
