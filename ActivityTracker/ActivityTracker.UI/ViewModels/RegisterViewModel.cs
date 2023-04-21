using System;
using System.IO;
using System.Windows.Input;

using ActivityTracker.Domain.Stores;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;

namespace ActivityTracker.UI.ViewModels
{
    public class Registration : PropertyStore
    {
        public Registration()
        {

        }

        public string UserId { get => Get<string>(); set => Set(value); }

        public string Password { get => Get<string>(); set => Set(value); }

        public string LicenseFilePath { get => Get<string>(); set => Set(value); }

        public string RegistrationKey { get => Get<string>(); set => Set(value); }
    }

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

        public string Password { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(CanRegister)); } }

        public string ConfirmPassword { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(CanRegister)); } }

        public string LicenseFilePath { get => Get<string>(); set => Set(value); }

        public string RegistrationKey { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(CanRegister)); } }

        #endregion

        #region Commands

        public ICommand SelectLicenseFileCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RegisterCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RequestLicenseCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Readonly Properties
        public bool CanRegister => !string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(Password) &&
                                   !string.IsNullOrWhiteSpace(ConfirmPassword) && Password == ConfirmPassword &&
                                   !string.IsNullOrWhiteSpace(RegistrationKey);
        #endregion

        #region Command Handlers

        private void OnSelectLicenseFile()
        {
            throw new NotImplementedException();
        }

        private void OnRegister()
        {
            string regFilename = "reg.json";
            string licFilename = "license.sal";
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "tmg");
            string regFilePath = Path.Combine(appDataDirectory, regFilename);
            string licFilePath = Path.Combine(appDataDirectory, licFilename);

            Registration registration = new()
            {
                UserId = UserId,
                Password = Password,
                LicenseFilePath = LicenseFilePath,
                RegistrationKey = RegistrationKey
            };

            IDataSerializer<Registration> serializer = SerializerFactory.GetSerializer<Registration>();

            string seializedData = serializer.Serialize(registration);

            if (!Directory.Exists(Path.GetDirectoryName(regFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(regFilePath));
            }

            //if (!File.Exists(regFilePath))
            //{

            //}

            File.WriteAllText(regFilePath, seializedData);

            RedirectToLogin();
        }

        private void OnRequestLicense()
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
            RequestLicenseCommand = new RelayCommand(OnRequestLicense, true);

        }
        #endregion
    }
}
