using System.Windows;

using ReInvented.UI.Views;

namespace ReInvented.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new TestingView();
            MainWindow.Show();
        }
    }
}
