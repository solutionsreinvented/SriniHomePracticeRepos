using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Services;

namespace ReInvented.ExcelInterop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Excel.Workbook workbook = WorkbookService.Create(@"C:\Users\masanams\Desktop", "Exported Data1.xlsm");










            base.OnStartup(e);
        }
    }
}
