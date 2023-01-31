using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.ViewModels;

using System.Windows;

namespace SlvParkview.FinanceManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NavigationService navigationService = new NavigationService();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationService)
            };

            MainWindow.Show();
        }
    }
}
