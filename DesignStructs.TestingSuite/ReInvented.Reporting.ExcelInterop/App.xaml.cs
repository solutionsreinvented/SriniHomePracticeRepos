using System.Windows;
using ReInvented.Domain.Reporting.Models;
using ReInvented.DataAccess;
using ReInvented.Reporting.ExcelInterop.Models;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using System;
using ReInvented.Reporting.ExcelInterop.ViewModels;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();/// { DataContext = new WebViewViewModel(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.html") };

            MainWindow.Show();

            //JsonDataSerializer<FLDReport> serializer = new JsonDataSerializer<FLDReport>();
            //FLDReport fldReport = serializer.Deserialize(@"C:\Users\masanams\Desktop\FLD.json");

            //FldWorkbook workbookService = new FldWorkbook(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm", fldReport);
            //workbookService.Create();

            //base.OnStartup(e);
        }

    }
}
