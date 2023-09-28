using System.Windows;

using ReInvented.Documentation.Reporting.ViewModels;
using ReInvented.Documentation.Reporting.Views;

namespace ReInvented.Documentation.Reporting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new HomeView() { DataContext = new MainViewModel() };
            MainWindow.Show();
        }
    }
}
