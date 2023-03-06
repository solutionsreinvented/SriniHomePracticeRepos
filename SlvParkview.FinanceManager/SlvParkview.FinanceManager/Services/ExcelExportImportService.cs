using System;
using System.Data;

using Microsoft.Office.Interop.Excel;

using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Services
{
    public static class ExcelExportImportService
    {
        public static void ExportFromExcelToJson()
        {
            Application xlApplication = new Application();

            ///excel.Visible = false;
            ///excel.DisplayAlerts = false;

            Workbook workbook = xlApplication.Workbooks.Add(Type.Missing);

            string path = @"C:\Users\masanams\Desktop\Export.xlsx";

            workbook.SaveAs(path);

            _ = xlApplication.Workbooks.Open(path);

            xlApplication.Quit();

            ///Worksheet aTowerSheet = workbook.ActiveSheet as Worksheet;

            ///workbook.Worksheets.Add(aTowerSheet);

            ///Worksheet aTowerSheet = workbook.ActiveSheet as Worksheet;
            ///Worksheet aTowerSheet = workbook.ActiveSheet as Worksheet;
            ///worksheet.Name = "A Block";
            ///Range range;


            ///Sheets sheets = workbook.Sheets;

            ///foreach (var sheet in sheets)
            ///{
            ///}
        }

        public static void ExportOutstandingDuesToExcel(BlockOutstandingsReport report)
        {
            int sRow = 3; int sCol = 3; int eRow = sRow + report.FlatInfoCollection.Count; int eCol = sCol + 5;

            System.Data.DataTable reportTable = GenerateDataTable(report);

            Application xlApplication = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            Workbook xlWorkbook = xlApplication.Workbooks.Add(Type.Missing);
            Worksheet xlWorksheet = xlWorkbook.ActiveSheet as Worksheet;

            xlWorksheet.Name = "Outstanding Dues";

            xlWorksheet.Range[xlWorksheet.Cells[sRow, sCol], xlWorksheet.Cells[sRow, eCol]].Merge();
            xlWorksheet.Cells[sRow, sCol] = report.DocumentTitle;

            for (int i = 0; i < reportTable.Columns.Count; i++)
            {
                xlWorksheet.Cells[sRow + 1, sCol + i] = reportTable.Columns[i].ColumnName;
            }

            int currentRow = sRow + 1;

            foreach (DataRow dataRow in reportTable.Rows)
            {
                currentRow += 1;

                for (int i = 0; i < reportTable.Columns.Count; i++)
                {
                    xlWorksheet.Cells[currentRow, sCol + i] = dataRow[i];
                }
            }

            Range printableArea = xlWorksheet.Range[xlWorksheet.Cells[sRow, sCol], xlWorksheet.Cells[eRow, eCol]];
            printableArea.EntireColumn.AutoFit();

            printableArea.Borders.LineStyle = XlLineStyle.xlContinuous;
            printableArea.Borders.Weight = 2d;

            xlWorkbook.SaveAs(@"C:\Users\masanams\Desktop\Outstandings Test Report.xlsx");
            xlWorkbook.Close();
            xlApplication.Quit();
        }

        private static System.Data.DataTable GenerateDataTable(BlockOutstandingsReport report)
        {
            System.Data.DataTable table = new System.Data.DataTable();

            _ = table.Columns.Add("Flat Number", typeof(string));
            _ = table.Columns.Add("Owner", typeof(string));
            _ = table.Columns.Add("Co-owner", typeof(string));
            _ = table.Columns.Add("Tenant", typeof(string));
            _ = table.Columns.Add("Residing", typeof(string));
            _ = table.Columns.Add("Outstanding", typeof(decimal));

            foreach (FlatInfo f in report.FlatInfoCollection)
            {
                _ = table.Rows.Add($"{f.Description}, {f.OwnedBy}, {f.CoOwnedBy}, {f.TenantName}, {f.ResidentType}, {f.CurrentOutstanding}");
            }

            return table;
        }

    }
}
