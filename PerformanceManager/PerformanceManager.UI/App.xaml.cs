using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Services;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;
using PerformanceManager.UI.ViewModels;

namespace PerformanceManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            NavigationStore navigationStore = new();

            ///navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            navigationStore.CurrentViewModel = new AdminDashboardViewModel(navigationStore);

            ///navigationStore.CurrentViewModel = new DashboardViewModel(navigationStore);

            MainWindow = new MainWindow()
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
