using System;
using System.IO;
using System.Windows;

using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Dialogs;
using ActivityTracker.UI.Stores;
using ActivityTracker.UI.ViewModels;

using ReInvented.DataAccess.Factories;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;

namespace ActivityTracker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<CreateProjectViewModel, CreateProjectView>();
            dialogService.Register<CreateActivityViewModel, CreateActivityView>();

            ///base.OnStartup(e);

            NavigationStore navigationStore = new(dialogService);

            if (GetRegistrationStatus() == RegistrationStatus.Unregistered)
            {
                navigationStore.ManageUserViewModel = new RegisterViewModel(navigationStore);
            }
            else
            {
                if (KeyIsVerified())
                {
                    navigationStore.ManageUserViewModel = new LoginViewModel(navigationStore);
                }
                else
                {
                    navigationStore.ManageUserViewModel = new RegisterViewModel(navigationStore);
                }
            }




            //navigationStore.CurrentViewModel = new AdminDashboardViewModel(navigationStore);

            //navigationStore.CurrentViewModel = new DashboardViewModel(navigationStore);

            MainWindow = new Home()
            {
                DataContext = new HomeViewModel(navigationStore)
                {
                    CloseCommand = new RelayCommand(OnClose, true),
                    MinimizeCommand = new RelayCommand(OnMinimize, true),
                    MaximizeRestoreCommand = new RelayCommand(OnMaximizeRestore, true)
                }

            };

            MainWindow.Show();

        }

        private bool KeyIsVerified()
        {
            return true;
        }

        private enum RegistrationStatus
        {
            Registered,
            Unregistered
        }

        private RegistrationStatus GetRegistrationStatus()
        {
            string regFilename = "reg.json";
            string licFilename = "license.sal";
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "tmg");
            string regFilePath = Path.Combine(appDataDirectory, regFilename);
            string licFilePath = Path.Combine(appDataDirectory, licFilename);

            return !File.Exists(regFilePath) || !File.Exists(licFilePath) ? RegistrationStatus.Unregistered : RegistrationStatus.Registered;
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
