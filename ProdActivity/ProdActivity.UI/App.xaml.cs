using System;
using System.IO;
using System.Windows;
using System.Threading;

using ProdActivity.UI.Commands;
using ProdActivity.UI.Dialogs;
using ProdActivity.UI.Stores;
using ProdActivity.UI.ViewModels;
using ProdActivity.UI.Models;

using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;

namespace ProdActivity.UI
{
    public partial class App : Application
    {
        private Timer _notificationTimer;
        private HomeViewModel _homeViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<CreateProjectViewModel, CreateProjectView>();
            dialogService.Register<CreateActivityViewModel, CreateActivityView>();

            NavigationStore navigationStore = new(dialogService);
            
            _homeViewModel = new HomeViewModel(navigationStore)
            {
                CloseCommand = new RelayCommand(OnClose, true),
                MinimizeCommand = new RelayCommand(OnMinimize, true),
                MaximizeRestoreCommand = new RelayCommand(OnMaximizeRestore, true)
            };

            var regStatus = GetRegistrationStatus();

            if (regStatus == RegistrationStatus.Unregistered)
            {
                navigationStore.ManageUserViewModel = new RegisterViewModel(navigationStore);
            }
            else
            {
                var reg = LoadRegistration();
                if (KeyIsVerified(reg))
                {
                    // Directly proceed to the application as Standard User
                    navigationStore.ManageUserViewModel = new LoginViewModel(navigationStore) { IsLoggedIn = true };
                    navigationStore.DashboardViewModel = new StandardDashboardViewModel(navigationStore);
                    StartBackgroundService();
                }
                else
                {
                    navigationStore.ManageUserViewModel = new RegisterViewModel(navigationStore);
                }
            }

            MainWindow = new Home()
            {
                DataContext = _homeViewModel
            };

            dialogService.SetOrChangeOwner(MainWindow);

            MainWindow.Show();
        }

        private void StartBackgroundService()
        {
            // Check every 5 minutes if it's time to notify
            _notificationTimer = new Timer(CheckNotificationTime, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        private DateTime _lastMorningNotification = DateTime.MinValue;
        private DateTime _lastEveningNotification = DateTime.MinValue;

        private void CheckNotificationTime(object state)
        {
            var reg = LoadRegistration();
            if (reg == null || !KeyIsVerified(reg)) return;

            var now = DateTime.Now;

            // Morning Notification around 9:00 AM
            if (now.Hour >= 9 && now.Hour < 12 && _lastMorningNotification.Date != now.Date)
            {
                ShowInAppToast("Good Morning!", "Don't forget to review and log your planned activities for today.");
                _lastMorningNotification = now;
            }

            // Evening Notification around 5:00 PM (17:00)
            if (now.Hour >= 17 && _lastEveningNotification.Date != now.Date)
            {
                ShowInAppToast("Evening Reminder", "Please update the progress of the activities you worked on today before logging off.");
                _lastEveningNotification = now;
            }
        }

        private void ShowInAppToast(string title, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _homeViewModel?.ShowNotification(title, message);
            });
        }

        private bool KeyIsVerified(ReInvented.Licensing.Core.Models.Registration reg)
        {
            if (reg == null || reg.Records == null) return false;

            var record = System.Linq.Enumerable.FirstOrDefault(reg.Records, r => r.Module == ReInvented.Licensing.Core.Enums.ApplicationModule.ProjectActivitiesManager);
            if (record == null) return false;

            return ReInvented.Licensing.Core.Services.AuthenticationService.ValidateRegistrationKey(record.RegistrationKey, record.LicenseFilePath);
        }

        private enum RegistrationStatus
        {
            Registered,
            Unregistered
        }

        private RegistrationStatus GetRegistrationStatus()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReInvented");
            string regFilePath = Path.Combine(appDataDirectory, "Registration.sareg");

            return !File.Exists(regFilePath) ? RegistrationStatus.Unregistered : RegistrationStatus.Registered;
        }

        private ReInvented.Licensing.Core.Models.Registration LoadRegistration()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReInvented");
            string regFilePath = Path.Combine(appDataDirectory, "Registration.sareg");
            
            if (File.Exists(regFilePath))
            {
                return ReInvented.DataAccess.Services.PersistenceService.TryReadFromFile<ReInvented.Licensing.Core.Models.Registration>(regFilePath);
            }
            return null;
        }

        private void OnMaximizeRestore()
        {
            Current.MainWindow.WindowState = Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnMinimize()
        {
            Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OnClose()
        {
            Current.MainWindow.Close();
        }
    }
}

