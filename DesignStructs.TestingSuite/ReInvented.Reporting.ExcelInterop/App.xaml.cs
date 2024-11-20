using System.Windows;
using ReInvented.Domain.Reporting.Models;
using ReInvented.DataAccess;
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

            FldWorkbook workbookService = new FldWorkbook(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm", fldReport);
            workbookService.Create();

            //Excel.Workbook workbook = WorkbookService.Create(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm", fldReport);







            base.OnStartup(e);
        }
    }
}
