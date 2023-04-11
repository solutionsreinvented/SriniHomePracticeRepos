using System.Windows;

using ReIn.NavPractice.ViewModels;

namespace ReIn.NavPractice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new HtmlContentView()
            {
                DataContext = new HtmlContentViewModel()
            };

            MainWindow.Show();
        }
    }
}
