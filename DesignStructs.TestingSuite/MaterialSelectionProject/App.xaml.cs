using System.Windows;

using MaterialSelectionProject.ViewModels;

namespace MaterialSelectionProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow() { DataContext = new MaterialSelectionViewModel() };
            MainWindow.Show();
        }
    }
}
