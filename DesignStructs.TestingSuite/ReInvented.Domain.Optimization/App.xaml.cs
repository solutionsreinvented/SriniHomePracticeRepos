using System;
using System.Windows;

using ReInvented.Domain.Optimization.ViewModels;
using ReInvented.Domain.Optimization.Views;
using ReInvented.Shared.Dialogs;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;
using ReInvented.Shared.ViewModels;

namespace ReInvented.Domain.Optimization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var guid = Guid.NewGuid();
            base.OnStartup(e);
            IDialogService dialogService = new DialogService();
            ConfigureDiloags(dialogService);

            PlatesOptimizationViewModel poViewModel = new PlatesOptimizationViewModel(dialogService);
            PlatesOptimizationView home = new PlatesOptimizationView() { DataContext = poViewModel};


            MainWindow = home;
            MainWindow.Show();

            dialogService.SetOrChangeOwner(home);
        }

        private void ConfigureDiloags(IDialogService dialogService)
        {
            dialogService.Register<MessageViewModel, MessageDialog>();
        }
    }
}
