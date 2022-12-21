using System.Windows.Input;

using PerformanceManager.Domain.Repositories;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;

namespace PerformanceManager.UI.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly UserRepository _usersRepository = new();

        public ChangePasswordViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            SubmitCommand = new RelayCommand(OnPasswordChange, true);
        }

        private void OnPasswordChange()
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }

        public bool CanChangePassword => UserExists() && PasswordsMatch();

        public int UserId
        {
            get => Get<int>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }

        public string CurrentPassword
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }

        public string NewPassword
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }

        public string ConfirmPassword
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }

        public ICommand SubmitCommand { get => Get<ICommand>(); set => Set(value); }

        private bool UserExists() => _usersRepository.GetById(UserId) != null;

        private bool PasswordsMatch() => !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && NewPassword == ConfirmPassword;

    }
}
