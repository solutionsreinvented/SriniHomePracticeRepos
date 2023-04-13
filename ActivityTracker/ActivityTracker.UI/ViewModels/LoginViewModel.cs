using System.Windows.Input;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Repositories;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class LoginViewModel : ManageUserViewModel
    {
        #region Private Fields
        private readonly UserRepository _usersRepository = new();
        #endregion

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
                _navigationStore.CurrentViewModel = new DashboardViewModel(_navigationStore)
                {

                };
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

        public IUser User { get => Get<IUser>(); private set => Set(value); }

        public string UserId
        {
            get => Get<string>();
            set
            {
                Set(value);
                User = GetUser(value);
                RaiseMultiplePropertiesChanged(nameof(ValidUser), nameof(UserExists));
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

        public bool ValidUser => UserExists && ValidPassword();

        public bool UserExists => User != null;

        private IUser GetUser(string userId)
        {
            _ = int.TryParse(userId, out int result);

            return _usersRepository.GetById(result);
        }

        private bool ValidPassword()
        {
            return User != null && User.Password == Password;

            ///return !string.IsNullOrEmpty(Password) && (Password.Length is >= 8 and <= 15) && Password.Any(c => char.IsDigit(c));
        }

        public bool IsLoggedIn { get => Get<bool>(); set => Set(value); }

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
