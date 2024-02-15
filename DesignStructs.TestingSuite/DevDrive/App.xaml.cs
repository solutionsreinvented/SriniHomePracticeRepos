using System.Windows;

namespace DevDrive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            HtmlToExcelExporter exporter = new HtmlToExcelExporter();
            exporter.Export();
        }
    }
}
