using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Services;
using ReInvented.Domain.Reporting.Models;
using ReInvented.DataAccess;

namespace ReInvented.ExcelInterop
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

            Excel.Workbook workbook = WorkbookService.Create(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm", fldReport);










            base.OnStartup(e);
        }
    }
}
