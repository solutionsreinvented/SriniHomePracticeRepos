using System.Windows.Input;

using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Repositories;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly UserRepository _usersRepository = new();

        public ChangePasswordViewModel(NavigationStore navigationStore, IDialogService dialogService)
            : base(navigationStore, dialogService)
        {
            SubmitCommand = new RelayCommand(OnPasswordChange, true);
        }

        private void OnPasswordChange()
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore, _dialogService);
        }

        public bool CanChangePassword => UserExists && CurrentPasswordMatches && NewPasswordsMatch();

        public string UserId
        {
            get => Get<string>();
            set
            {
                Set(value);
                User = GetUser(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }

        public IUser User { get => Get<IUser>(); private set => Set(value); }

        public string CurrentPassword
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(CanChangePassword), nameof(CurrentPasswordMatches));
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

        private IUser GetUser(string userId)
        {
            _ = int.TryParse(userId, out int result);

            return _usersRepository.GetById(result);
        }

        public ICommand SubmitCommand { get => Get<ICommand>(); set => Set(value); }

        private bool UserExists => User != null;

        public bool CurrentPasswordMatches => User.Password == CurrentPassword;

        private bool NewPasswordsMatch() => !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && NewPassword == ConfirmPassword;

    }
}
