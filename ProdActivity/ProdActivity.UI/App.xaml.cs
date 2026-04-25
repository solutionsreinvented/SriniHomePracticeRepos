using System;
using System.IO;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

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
        private DispatcherTimer _licenseMonitorTimer;
        private DispatcherTimer _backgroundServiceTimer;
        private HomeViewModel _homeViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Check for background startup flag
            bool isBackgroundService = false;
            foreach (var arg in e.Args)
            {
                if (arg.Equals("--background", StringComparison.OrdinalIgnoreCase))
                {
                    isBackgroundService = true;
                    break;
                }
            }

            if (isBackgroundService)
            {
                RunAsBackgroundService();
                return;
            }

            // Normal Startup
            RegisterBackgroundServiceInStartup();

            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<CreateProjectViewModel, CreateProjectView>();
            dialogService.Register<CreateActivityViewModel, CreateActivityView>();
            dialogService.Register<AddUserViewModel, AddUserView>();

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

            StartContinuousLicenseMonitor(navigationStore);
        }

        private void RunAsBackgroundService()
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Check immediately on startup
            CheckAndShowDesktopNotification();

            // Check every minute if it's 5:30 PM
            _backgroundServiceTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
            _backgroundServiceTimer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                if (now.Hour == 17 && now.Minute >= 30 && now.Minute <= 35) // 5:30 PM window
                {
                    CheckAndShowDesktopNotification();
                }
            };
            _backgroundServiceTimer.Start();
        }

        private void CheckAndShowDesktopNotification()
        {
            var reg = LoadRegistration();
            if (reg == null || !KeyIsVerified(reg)) return; // Don't notify if unregistered

            // Check if completed today
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\ReInvented\ProdActivity");
                if (key != null)
                {
                    var lastDateStr = key.GetValue("LastNotificationCompletedDate") as string;
                    if (!string.IsNullOrEmpty(lastDateStr) && lastDateStr == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                    {
                        return; // Already completed today
                    }
                }
            }
            catch { }

            // Ensure only one window exists
            foreach (Window w in Application.Current.Windows)
            {
                if (w is ProdActivity.UI.Views.DesktopNotifierWindow) return;
            }

            var notifier = new ProdActivity.UI.Views.DesktopNotifierWindow();
            notifier.Show();
        }

        private void RegisterBackgroundServiceInStartup()
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string command = $"\"{exePath}\" --background";
                key?.SetValue("ProdActivityNotifier", command);
            }
            catch { }
        }

        private void StartContinuousLicenseMonitor(NavigationStore navigationStore)
        {
            _licenseMonitorTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(10) };
            _licenseMonitorTimer.Tick += (s, e) =>
            {
                var reg = LoadRegistration();
                if (!KeyIsVerified(reg))
                {
                    // Kick out to register screen
                    navigationStore.DashboardViewModel = null;
                    navigationStore.ManageUserViewModel = new RegisterViewModel(navigationStore);
                }
            };
            _licenseMonitorTimer.Start();
        }

        private void StartBackgroundService()
        {
            // Deprecated - using RunAsBackgroundService for desktop notification
        }

        private DateTime _lastMorningNotification = DateTime.MinValue;
        private DateTime _lastEveningNotification = DateTime.MinValue;

        private void CheckNotificationTime(object state)
        {
            // Deprecated in favor of the new DesktopNotifierWindow
        }

        private void ShowInAppToast(string title, string message)
        {
            // Keep if needed elsewhere, but no longer driven by timer
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

