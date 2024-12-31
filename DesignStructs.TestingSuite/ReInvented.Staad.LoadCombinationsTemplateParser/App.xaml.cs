using System.Windows;

using ReInvented.StaadPro.Interactivity.ViewModels;

namespace ReInvented.Staad.LoadCombinationsTemplateParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow() { DataContext = new LoadCombinationsDefinitionParserViewModel() };
            MainWindow.Show();
        }
    }
}
