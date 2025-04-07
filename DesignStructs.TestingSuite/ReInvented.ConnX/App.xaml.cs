using ReInvented.ConnX.ViewModels;
using ReInvented.ConnX.Views;
using System.Windows;

namespace ReInvented.ConnX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new HomeView() { DataContext = new HomeViewModel() };
            MainWindow.Show();
        }
    }
}
