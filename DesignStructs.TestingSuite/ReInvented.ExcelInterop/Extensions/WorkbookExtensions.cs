using System.IO;
using System.Runtime.InteropServices;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInterop.Extensions
{
    public static class WorkbookExtensions
    {
        public static void SaveWorkbookWithOverride(this Excel.Workbook workbook, string savePath, string fileName)
        {
            string fullPath = Path.Combine(savePath, fileName);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (IOException)
                {
                    workbook.Application.Quit();
                    _ = Marshal.ReleaseComObject(workbook);
                    workbook = null;

                    File.Delete(fullPath);
                }
            }

            workbook.SaveAs(fullPath, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
        }
    }
}
