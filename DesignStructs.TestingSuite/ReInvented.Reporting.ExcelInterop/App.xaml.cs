using System.Windows;

using ReInvented.DataAccess;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Reporting.ExcelInterop.Models;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            JsonDataSerializer<FLDReport> serializer = new JsonDataSerializer<FLDReport>();
            FLDReport fldReport = serializer.Deserialize(@"C:\Users\masanams\Desktop\FLD.json");

            FldWorkbook fldWorkbook = new FldWorkbook(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm", fldReport);
            //fldWorkbook.Create();

            MainWindow = new MainWindow(fldWorkbook);/// { DataContext = new WebViewViewModel(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.html") };

            MainWindow.Show();


            //base.OnStartup(e);
        }

    }
}
