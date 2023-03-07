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

            ///MainViewModel mainViewModel = new MainViewModel();

            MainWindow = new MainWindow();

            //MainWindow = new MainWindow()
            //{
            //    DataContext = mainViewModel
            //};


            MainWindow.Show();
        }
    }
}
