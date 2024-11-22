using System.Windows;

using ReInvented.Reporting.ExcelInterop.Services;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string htmlFilePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\SvgToPng\03. Reports\3913A0TR036CV101r4.html";

            var result = FldWorkbookGenerationService.GenerateExcelDocumentAsync(htmlFilePath, null);

            //MainWindow = new MainWindow(fldWorkbook);/// { DataContext = new WebViewViewModel(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.html") };

            //MainWindow.Show();


            //base.OnStartup(e);
        }

    }
}
