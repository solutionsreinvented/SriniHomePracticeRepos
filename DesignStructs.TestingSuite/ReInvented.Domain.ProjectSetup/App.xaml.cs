using System.Windows;

using ReInvented.Domain.ProjectSetup.ViewModels;
using ReInvented.Domain.ProjectSetup.Views;

namespace ReInvented.Domain.ProjectSetup
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
