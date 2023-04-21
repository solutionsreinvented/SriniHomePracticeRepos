using System;
using System.Windows.Input;

using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class RegisterViewModel : ManageUserViewModel
    {
        #region Parameterized Constructor
        public RegisterViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Initialize();
        }
        #endregion

        #region Public Properties

        public string UserId { get => Get<string>(); set { Set(value); User = GetUser(value); } }

        public string Password { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(PasswordsMatch)); } }

        public string ConfirmPassword { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(PasswordsMatch)); } }

        public string LicenseFilePath { get => Get<string>(); set => Set(value); }

        public string RegistrationKey { get => Get<string>(); set => Set(value); }


        #endregion

        #region Commands

        public ICommand SelectLicenseFileCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RegisterCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Readonly Properties
        public bool PasswordsMatch => Password == ConfirmPassword;
        #endregion

        #region Command Handlers

        private void OnSelectLicenseFile()
        {
            throw new NotImplementedException();
        }

        private void OnRegister()
        {
            throw new NotImplementedException();
        }

        private void RedirectToLogin()
        {
            _navigationStore.ManageUserViewModel = new LoginViewModel(_navigationStore) { UserId = User.Id.ToString() };
        }

        #endregion

        #region Abstract Methods Implementation
        protected override void Initialize()
        {
            IsLoggedIn = false;
            SelectLicenseFileCommand = new RelayCommand(OnSelectLicenseFile, true);
            RegisterCommand = new RelayCommand(OnRegister, true);
        }

        #endregion
    }
}
