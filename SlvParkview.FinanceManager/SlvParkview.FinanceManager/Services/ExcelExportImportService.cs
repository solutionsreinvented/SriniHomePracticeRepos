using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;


using Microsoft.Office.Interop.Excel;

using ReInvented.DataAccess.Enums;
using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Services
{
    public static class ExcelExportImportService
    {
        private static readonly string _filesDirectory = @"C:\Users\masanams\source\SriniHomePracticeRepos\SlvParkview.FinanceManager\SlvParkview.FinanceManager\Move to Bin";
        ///private static readonly string _filesDirectory = @"C:\Users\srini\source\repos\SlvParkview.FinanceManager\SlvParkview.FinanceManager\Move to Bin";


        public static void ExportFlatDataFromExcelToJson(string sourceFilePath, string destinationFilePath, string towerName)
        {
            const int sRow = 1;
            const int sCol = 1;

            int eRow;


            Application xlApplication = ExcelApplicationService.GetApplication();

            Workbook xlWorkbook = xlApplication.Workbooks.Open(sourceFilePath);
            Worksheet xlWorksheet = xlWorkbook.Worksheets["Block Details"];

            int currentRow = sRow;

            while (xlWorksheet.Cells[currentRow, sCol].Value2 != string.Empty && xlWorksheet.Cells[currentRow, sCol].Value2 != null)
            {
                currentRow++;
            }

            eRow = currentRow - 1;

            Block block = new Block() { Name = towerName };
            block.Flats = new List<Flat>();
            block.LastUpdated = DateTime.Today;
            block.PenaltyCommencesFrom = new DateTime(2023, 02, 01);

            DateTime accountOpenedOn = new DateTime(2023, 02, 10);

            for (int rowIndex = sRow + 1; rowIndex <= eRow; rowIndex++)
            {
                string flatDescription = xlWorksheet.Cells[rowIndex, sCol].Value2;

                Flat flat = new Flat(block, xlWorksheet.Cells[rowIndex, sCol + 1].Value2)
                {
                    AccountOpenedOn = accountOpenedOn,
                    Description = flatDescription,
                    Number = flatDescription.Substring(1, flatDescription.Length - 1),
                    CoOwnedBy = xlWorksheet.Cells[rowIndex, sCol + 2].Value2,
                    TenantName = xlWorksheet.Cells[rowIndex, sCol + 3].Value2,
                    ResidentType = Enum.Parse(typeof(ResidentType), xlWorksheet.Cells[rowIndex, sCol + 4].Value2),
                    OpeningBalance = (decimal)xlWorksheet.Cells[rowIndex, sCol + 5].Value2,
                    Expenses = new ObservableCollection<Expense>(),
                    Payments = new ObservableCollection<Payment>()
                };

                block.Flats.Add(flat);
            }

            IDataSerializer<Block> seriailzer = SerializerFactory.GetSerializer<Block>(DataFileFormat.Json);

            string seriailzedData = seriailzer.Serialize(block);


            File.WriteAllText(destinationFilePath, seriailzedData);


            xlWorkbook.Close();
            xlApplication.Quit();

            ExcelApplicationService.ReleaseObject(xlWorksheet);
            ExcelApplicationService.ReleaseObject(xlWorkbook);
            ExcelApplicationService.ReleaseObject(xlApplication);

        }

        public static void ExportOutstandingDuesToExcel(BlockOutstandingsReport report)
        {
            Application xlApplication = ExcelApplicationService.GetApplication();

            Workbook xlWorkbook = xlApplication.Workbooks.Add(Type.Missing);
            xlWorkbook = CopyDataToWorkbook(xlWorkbook, report);

            xlApplication.ActiveWindow.DisplayGridlines = false;


            xlWorkbook.SaveAs($"{Path.Combine(_filesDirectory, report.DocumentTitle)}.xlsx");
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
            _ = table.Columns.Add("Outstanding", typeof(string));

            foreach (FlatInfo f in report.FlatInfoCollection)
            {
                _ = table.Rows.Add(f.Description, f.OwnedBy, f.CoOwnedBy, f.TenantName, f.ResidentType, f.CurrentOutstanding);
            }

            return table;
        }

        private static Workbook CopyDataToWorkbook(Workbook activeWorkbook, BlockOutstandingsReport report)
        {
            int sRow = 3; int sCol = 3; int eRow = sRow + report.FlatInfoCollection.Count; int eCol = sCol + 5;

            System.Data.DataTable reportTable = GenerateDataTable(report);

            Worksheet xlWorksheet = activeWorkbook.ActiveSheet as Worksheet;

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

            xlWorksheet.Range[xlWorksheet.Cells[sRow, sCol], xlWorksheet.Cells[sRow, eCol]].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range headerArea = xlWorksheet.Range[xlWorksheet.Cells[sRow, sCol], xlWorksheet.Cells[sRow + 1, eCol]];
            headerArea.Font.Bold = true;

            Range printableArea = xlWorksheet.Range[xlWorksheet.Cells[sRow, sCol], xlWorksheet.Cells[eRow + 1, eCol]];
            printableArea.EntireColumn.AutoFit();

            printableArea.Borders.LineStyle = XlLineStyle.xlContinuous;
            printableArea.Borders.Weight = 2d;

            return activeWorkbook;
        }

    }
}
