using System.Windows.Input;

using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Repositories;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;

namespace PerformanceManager.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserRepository _usersRepository = new();

        public LoginViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            ChangePasswordCommand = new RelayCommand(OnChangePassword, true);
            LogInCommand = new RelayCommand(OnLogIn, true);
        }

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

        private IUser GetUser(string userId)
        {
            _ = int.TryParse(userId, out int result);

            return _usersRepository.GetById(result);
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

        private bool ValidPassword()
        {
            return User != null && User.Password == Password;

            ///return !string.IsNullOrEmpty(Password) && (Password.Length is >= 8 and <= 15) && Password.Any(c => char.IsDigit(c));
        }

        public bool IsLoggedIn { get => Get<bool>(); set => Set(value); }

        public ICommand ChangePasswordCommand { get; set; }

        public ICommand LogInCommand { get; set; }

    }
}
