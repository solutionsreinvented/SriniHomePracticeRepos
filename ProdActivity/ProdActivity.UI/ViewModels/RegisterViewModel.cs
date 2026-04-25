using System;
using System.IO;
using System.Windows.Input;

using ProdActivity.UI.Base;
using ProdActivity.UI.Commands;
using ProdActivity.UI.Models;
using ProdActivity.UI.Stores;

using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;

namespace ProdActivity.UI.ViewModels
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
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "ProdActivity License (*.pamlic)|*.pamlic|All Files (*.*)|*.*",
                Title = "Select License File"
            };

            if (dialog.ShowDialog() == true)
            {
                LicenseFilePath = dialog.FileName;
            }
        }

        private void OnRegister()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReInvented");
            string registrationFile = Path.Combine(appDataDirectory, "Registration.sareg");

            ReInvented.Licensing.Core.Models.License license = ReInvented.DataAccess.Services.PersistenceService.TryReadFromFile<ReInvented.Licensing.Core.Models.License>(LicenseFilePath);

            if (license == null)
            {
                System.Windows.MessageBox.Show("The selected license file could not be read. Please select a valid license file.", "Invalid license file", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (ReInvented.Licensing.Core.Services.AuthenticationService.ValidateRegistrationKey(RegistrationKey, LicenseFilePath))
            {
                ReInvented.Licensing.Core.Models.Registration registration;

                if (File.Exists(registrationFile))
                {
                    registration = ReInvented.DataAccess.Services.PersistenceService.TryReadFromFile<ReInvented.Licensing.Core.Models.Registration>(registrationFile);
                }
                else
                {
                    registration = new ReInvented.Licensing.Core.Models.Registration();
                }

                if (registration == null)
                {
                    registration = new ReInvented.Licensing.Core.Models.Registration();
                }

                if (registration.Records == null)
                {
                    registration.Records = new System.Collections.Generic.HashSet<ReInvented.Licensing.Core.Models.RegistrationRecord>();
                }

                string targetLicenseFilePath = Path.Combine(appDataDirectory, "license.pamlic");

                var regRecord = new ReInvented.Licensing.Core.Models.RegistrationRecord()
                { 
                    Module =  license.ModuleType, 
                    Type = license.LicenseType,
                    LicenseFilePath = targetLicenseFilePath, 
                    RegistrationKey = RegistrationKey 
                };

                registration.Records.Add(regRecord);

                if (!string.Equals(LicenseFilePath, targetLicenseFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (!Directory.Exists(appDataDirectory))
                    {
                        Directory.CreateDirectory(appDataDirectory);
                    }
                    File.Copy(LicenseFilePath, targetLicenseFilePath, true);
                }

                ReInvented.DataAccess.Services.PersistenceService.WriteToFile(registrationFile, registration);

                System.Windows.MessageBox.Show("The registration is successfully completed. You can start using the product. You will now be redirected to the dashboard.", "Registration successful", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                RedirectToLogin();
            }
            else
            {
                System.Windows.MessageBox.Show("The registration key provided is invalid. Please contact the publisher.", "Invalid registration key", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnRequestLicense()
        {
            var window = new ReInvented.Licensing.Core.Views.LicenseInputGenerationView();
            window.ShowDialog();
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
