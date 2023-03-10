using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

using OfficeOpenXml;

namespace Techtural.ExploringNuGet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static void ExportFlatDataFromExcelToJson(string sourceFilePath, string destinationFilePath, string towerName)
        {
            var block = new Block { Name = towerName, Flats = new List<Flat>(), LastUpdated = DateTime.Today, PenaltyCommencesFrom = new DateTime(2023, 02, 01) };

            using (var xlPackage = new ExcelPackage(new FileInfo(sourceFilePath)))
            {
                var xlWorksheet = xlPackage.Workbook.Worksheets["Block Details"];

                RetrieveBlockFlatsDataFromExcelSpreadsheet(xlWorksheet, block);

                IDataSerializer<Block> serializer = SerializerFactory.GetSerializer<Block>(DataFileFormat.Json);

                string serializedData = serializer.Serialize(block);

                File.WriteAllText(destinationFilePath, serializedData);
            }
        }

        private static void RetrieveBlockFlatsDataFromExcelSpreadsheet(ExcelWorksheet xlWorksheet, Block block)
        {
            const int sRow = 2;
            const int flatDescriptionCol = 1;
            const int flatNumberCol = 2;
            const int flatCoOwnedByCol = 3;
            const int flatTenantNameCol = 4;
            const int flatResidentTypeCol = 5;
            const int flatOpeningBalanceCol = 6;

            int eRow = xlWorksheet.Dimension.End.Row;

            for (int rowIndex = sRow; rowIndex <= eRow; rowIndex++)
            {
                string flatDescription = xlWorksheet.Cells[rowIndex, flatDescriptionCol].GetValue<string>();

                Flat flat = new Flat(block, xlWorksheet.Cells[rowIndex, flatNumberCol].GetValue<string>())
                {
                    AccountOpenedOn = new DateTime(2023, 02, 01),
                    Description = flatDescription,
                    Number = flatDescription.Substring(1),
                    CoOwnedBy = xlWorksheet.Cells[rowIndex, flatCoOwnedByCol].GetValue<string>(),
                    TenantName = xlWorksheet.Cells[rowIndex, flatTenantNameCol].GetValue<string>(),
                    ResidentType = Enum.Parse<ResidentType>(xlWorksheet.Cells[rowIndex, flatResidentTypeCol].GetValue<string>()),
                    OpeningBalance = xlWorksheet.Cells[rowIndex, flatOpeningBalanceCol].GetValue<decimal>(),
                    Expenses = new ObservableCollection<Expense>(),
                    Payments = new ObservableCollection<Payment>()
                };

                block.Flats.Add(flat);
            }
        }

    }
}
