using System.Windows;

using ReInvented.Domain.Optimization.Views;

namespace ReInvented.Domain.Optimization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new PlatesOptimizationView();
            MainWindow.Show();
        }
    }
}
