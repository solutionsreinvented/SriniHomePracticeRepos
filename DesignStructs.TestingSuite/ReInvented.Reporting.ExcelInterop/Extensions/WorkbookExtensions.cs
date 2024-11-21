using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

namespace ReInvented.Reporting.ExcelInterop.Extensions
{
    public static class WorkbookExtensions
    {
        public static void SaveWorkbookWithOverride(this Workbook workbook, string savePath, string fileName)
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

            workbook.SaveAs(fullPath, XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
        }
    }
}
