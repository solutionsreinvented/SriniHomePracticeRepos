using System.Windows;

using ActivityTracker.Domain.Services;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Dialogs;
using ActivityTracker.UI.Stores;
using ActivityTracker.UI.ViewModels;

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

            ///navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            navigationStore.CurrentViewModel = new AdminDashboardViewModel(navigationStore);

            ///navigationStore.CurrentViewModel = new DashboardViewModel(navigationStore);

            MainWindow = new Home()
            {
                DataContext = new MainViewModel(navigationStore)
                {
                    CloseCommand = new RelayCommand(OnClose, true),
                    MinimizeCommand = new RelayCommand(OnMinimize, true),
                    MaximizeRestoreCommand = new RelayCommand(OnMaximizeRestore, true)
                }

            };

            MainWindow.Show();

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
