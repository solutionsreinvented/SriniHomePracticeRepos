using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.TestingSuite
{
    public class ExcelInterop
    {
        Excel.Application _excelApp;
        readonly Excel.Workbook _workbook;
        readonly Dictionary<string, Excel.Workbook> _workbooks = new Dictionary<string, Excel.Workbook>();

        public void Design()
        {
            _excelApp = new Excel.Application
            {
                DisplayAlerts = false
            };

            string fileDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\Excel";

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    string filename = $"Wookbook {i + 1}";
                    Excel.Workbook workbook = _excelApp.Workbooks.Open(Path.Combine(fileDirectory, filename));
                    _workbooks.Add(filename, workbook);


                    //Excel.Workbook _workbook = _excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATExcel4MacroSheet);
                    //_workbook.SaveAs(Path.Combine(fileDirectory, filename), Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
                    //_workbook.Close();
                }
                _workbooks.Values.ToList().ForEach(wb => FillData(25, wb));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ///_workbooks.Values.ToList().ForEach(wb => wb.Close());
                _excelApp.Quit();
                Marshal.ReleaseComObject(_excelApp);
            }

        }

        private void FillData(int count, Excel.Workbook workbook)
        {
            IEnumerable<int> range = Enumerable.Range(0, count);

            Excel.Worksheet worksheet = workbook.Worksheets["Macro1"] as Excel.Worksheet;

            for (int row = 0; row < range.Max(); row++)
            {
                worksheet.Cells[row, "C"] = row;
            }

            workbook.Save();

            Marshal.ReleaseComObject(worksheet);

            workbook.Close();
        }
    }
}
