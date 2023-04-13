using System.Windows.Input;

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

        #region Command Handlers
        private void OnLogIn()
        {
            if (ValidUser)
            {
                IsLoggedIn = true;

                _navigationStore.CurrentViewModel = new DashboardViewModel(_navigationStore) { };
            }
        }

        private void OnChangePassword()
        {
            _navigationStore.CurrentViewModel = new ChangePasswordViewModel(_navigationStore)
            {
                UserId = UserId,
            };
        }
        #endregion

        #region Public Properties
        public string UserId
        {
            get => Get<string>();
            set { Set(value); User = GetUser(value); RaiseMultiplePropertiesChanged(nameof(ValidUser), nameof(UserExists)); }
        }

        public string Password
        {
            get => Get<string>();
            set { Set(value); RaiseMultiplePropertiesChanged(nameof(ValidUser), nameof(UserExists)); }
        }

        public bool IsLoggedIn { get => Get<bool>(); set => Set(value); }
        #endregion

        #region Readonly Properties
        public bool ValidUser => UserExists && ValidPassword(); 
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

        #region Abstract Methods Implementation
        protected override void Initialize()
        {
            ChangePasswordCommand = new RelayCommand(OnChangePassword, true);
            LogInCommand = new RelayCommand(OnLogIn, true);
        }
        #endregion

    }
}
