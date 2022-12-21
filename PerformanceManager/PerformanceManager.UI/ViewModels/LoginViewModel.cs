using System;
using System.Linq;
using System.Windows.Input;

using PerformanceManager.Domain.Base;
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
                CurrentPassword = Password
            };
        }

        public IUser User { get => Get<IUser>(); private set => Set(value); }

        public int UserId
        {
            get => Get<int>();
            set
            {
                Set(value);
                User = _usersRepository.GetById(value);
                OnPropertyChanged(nameof(ValidUser));
            }
        }

        public string Password
        {
            get => Get<string>();
            set
            {
                Set(value);
                OnPropertyChanged(nameof(ValidUser));
            }
        }

        public bool ValidUser => UserExists() && ValidPassword();

        private bool UserExists() => User != null;

        private bool ValidPassword()
        {
            if (User == null)
            {
                return false;
            }

            return User.Password == Password;

            ///return !string.IsNullOrEmpty(Password) && (Password.Length is >= 8 and <= 15) && Password.Any(c => char.IsDigit(c));
        }

        public bool IsLoggedIn { get => Get<bool>(); set => Set(value); }

        public ICommand ChangePasswordCommand { get; set; }

        public ICommand LogInCommand { get; set; }

    }
}
