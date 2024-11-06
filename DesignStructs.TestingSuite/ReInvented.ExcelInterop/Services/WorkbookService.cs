using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.Reporting.Models;
using ReInvented.ExcelInterop.Extensions;


namespace ReInvented.ExcelInterop.Services
{
    public class WorkbookService
    {
        #region Private Static Fields

        private const string _inputBackground = "#F8EEC8";
        private const string _highlight = "#F5E6B1";

        #endregion


        #region Parameterized Constructor

        public WorkbookService(string savePath, string fileName, FoundationLoadData foundationLoadData)
        {
            if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Either {nameof(savePath)} or {nameof(fileName)} is invalid. Please provide valid data.");
            }

            if (!Directory.Exists(savePath))
            {
                _ = Directory.CreateDirectory(savePath);
            }

            FoundationLoadData = foundationLoadData;
            SavePath = savePath;
            FileName = fileName;
        }

        #endregion

        #region Public Properties

        public FoundationLoadData FoundationLoadData { get; private set; }

        public string SavePath { get; private set; }

        public string FileName { get; private set; }

        //public Application Application { get; set; }

        public Workbook Workbook { get; private set; }

        //public Worksheet Worksheet { get; set; }

        #endregion

        #region Instance Methods

        public Workbook Create()
        {
            Workbook = Create(SavePath, FileName);
            return Workbook;
        }

        public void Dispose()
        {
            Workbook.Close(false);
            //Application.Quit();

            //_ = Marshal.ReleaseComObject(Worksheet);
            _ = Marshal.ReleaseComObject(Workbook);
            //_ = Marshal.ReleaseComObject(Application);
        }

        #endregion

        public static Workbook Create(string savePath, string fileName)
        {
            Application excelApp = new Application { Visible = true };

            Workbook workbook = excelApp.Workbooks.Add();
            workbook.SaveAs(Path.Combine(savePath, fileName), XlFileFormat.xlOpenXMLWorkbookMacroEnabled);

            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            worksheet.Name = "Exported Data";

            worksheet.SetTemplateDefaults();

            CreateHeader(worksheet);

            workbook.Save();

            return workbook;
        }

        public static void CreateHeader(Worksheet worksheet)
        {
            Dictionary<string, string> rangeCaptionPairs = new Dictionary<string, string>()
            {
                { "A1", "Document No."}, { "A2", "Title"}, { "A3", "Project"}, { "A4", "Client"},
                { "S1", "Originator"}, { "S2", "Checker"}, { "S3", "Approver"},{ "S4", "<Template Name>"},
                { "Z1", "Life Cycle Status"},
                { "Z2", "Code"}, { "Z3", "Revision"}, { "Z4", "Date"}
            };

            Dictionary<string, string> rangeValuePairs = new Dictionary<string, string>()
            {
                { "G1", "<Document No.>"}, { "G2", "<Title>"}, { "G3", "<Project>"}, { "G4", "<Client>"},
                { "W1", "<Originator>"}, { "W2", "<Checker>"}, { "W3", "<Approver>"},
                { "AD2", "<Code>"}, { "AD3", "<Revision>"}, { "AD4", DateTime.Now.ToString("dd-MMM-yyyy")}
            };

            _ = worksheet.Merge(new HashSet<string>() { "A1:E1", "A2:E2", "A3:E3", "A4:E4", "G1:R1", "G2:R2", "G3:R3", "G4:R4", "S1:U1", "S2:U2", "S3:U3" })
                         .Merge(new HashSet<string>() { "W1:Y1", "W2:Y2", "W3:Y3", "Z2:AB2", "Z3:AB3", "Z4:AB4", "AD2:AG2", "AD3:AG3", "AD4:AG4", "S4:Y4", "Z1:AG1" })
                         .Fill(rangeCaptionPairs)
                         .Fill(rangeValuePairs)
                         .Fill(new HashSet<string>() { "F1", "F2", "F3", "F4", "V1", "V2", "V3", "AC2", "AC3", "AC4" }, ":")
                         .AlignLeftIndented(new HashSet<string>() { "A1", "A2", "A3", "A4", "S1", "S2", "S3", "Z2", "Z3", "Z4" }, 1)
                         .AlignCenter(new HashSet<string>() { "F1", "F2", "F3", "F4", "V1", "V2", "V3", "AC2", "AC3", "AC4" })
                         .AlignCenter(new HashSet<string>() { "W1", "W2", "W3", "S4", "Z1", "AD2", "AD3", "AD4" })
                         .BordersAroundAndInsideHorizontal(new HashSet<string>() { "A1:R4", "S1:Y3", "S4:Y4", "Z1:AG1", "Z2:AG4" })
                         .SetNumberFormat("AD4", "dd-MM-yyyy")
                         .SetBackgroundColor(new HashSet<string>() { "S4:Y4", "Z1:AG1" }, _highlight);

        }
    }
}
