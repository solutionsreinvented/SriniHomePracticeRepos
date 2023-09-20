using System.Windows;

using ReInvented.Animations.ViewModels;
using ReInvented.Animations.Views;

namespace ReInvented.Animations
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new Home() { DataContext = new HomeViewModel() };
            MainWindow.Show();
        }
    }
}
