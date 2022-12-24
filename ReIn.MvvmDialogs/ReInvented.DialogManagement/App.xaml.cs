using ReInvented.DialogManagement.Interfaces;
using ReInvented.DialogManagement.Services;
using ReInvented.DialogManagement.ViewModels;
using ReInvented.DialogManagement.Views;
using System.Windows;

namespace ReInvented.DialogManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<SectionSelectionViewModel, SectionSelectionView>();

            MainWindowViewModel viewModel = new MainWindowViewModel(dialogService);
            MainWindow view = new MainWindow() { DataContext = viewModel };

            _ = view.ShowDialog();
        }
    }
}
