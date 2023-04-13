using System;
using System.Windows.Input;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Repositories;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly UserRepository _usersRepository = new();

        public ChangePasswordViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            GoBackCommand = new RelayCommand(OnGoBack, true);
            SubmitCommand = new RelayCommand(OnPasswordChange, true);
        }

        private void OnPasswordChange()
        {
            throw new NotImplementedException();
            ///_navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }

        private void OnGoBack()
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore) { UserId = User.Id.ToString() };
        }

        public bool CanChangePassword => UserExists && CurrentPasswordMatches && NewPasswordsMatch();

        public string UserId
        {
            get => Get<string>();
            set { Set(value); User = GetUser(value); RaisePropertyChanged(nameof(CanChangePassword)); }
        }

        public IUser User { get => Get<IUser>(); private set => Set(value); }

        public string CurrentPassword
        {
            get => Get<string>();
            set { Set(value); RaiseMultiplePropertiesChanged(nameof(CanChangePassword), nameof(CurrentPasswordMatches)); }
        }

        public string NewPassword
        {
            get => Get<string>();
            set { Set(value); RaisePropertyChanged(nameof(CanChangePassword)); }
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

        public ICommand GoBackCommand { get => Get<ICommand>(); set => Set(value); }

        private bool UserExists => User != null;

        public bool CurrentPasswordMatches => User.Password == CurrentPassword;

        private bool NewPasswordsMatch()
        {
            return !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && NewPassword == ConfirmPassword;
        }
    }
}
