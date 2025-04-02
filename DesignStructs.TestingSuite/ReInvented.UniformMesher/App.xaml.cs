using System.Windows;

using ReInvented.UniformMesher.ViewModels;
using ReInvented.UniformMesher.Views;

namespace ReInvented.UniformMesher
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
