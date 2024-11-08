using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

using ReInvented.Shared;
using ReInvented.Domain.Reporting.Models;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Entities;
using System.Linq;

namespace ReInvented.ExcelInterop.Services
{
    public class WorkbookService
    {
        #region Private Static Fields

        private const string _inputBackground = "#F8EEC8";
        private const string _highlight = "#F5E6B1";

        private static readonly Dictionary<string, string> _captionRanges = new Dictionary<string, string>()
        {
            {"Document No.", "A1:E1" }, {"Title", "A2:E2" }, {"Project", "A3:E3" }, {"Client", "A4:E4" },
            {"Originator", "U1:X1" }, {"Checker", "U2:X2" }, {"Approver", "U3:X3" },
            {"Code", "AC2:AE2" }, {"Revision", "AC3:AE3" }, {"Date", "AC4:AE4" }
        };

        private static readonly Dictionary<string, string> _valueRangesLeftIndented = new Dictionary<string, string>()
        {
            {"Document No.", "G1:T1" }, {"Title", "G2:T2" }, {"Project", "G3:T3" }, {"Client", "G4:T4" }
        };

        private static readonly Dictionary<string, string> _valueRangesCenter = new Dictionary<string, string>()
        {
            {"Originator", "Z1:AB1" }, {"Checker", "Z2:AB2" }, {"Approver", "Z3:AB3" },
            {"Code", "AG2:AJ2" }, {"Revision", "AG3:AJ3" }, {"Date", "AG4:AJ4" }
        };

        private static readonly Dictionary<string, string> _uniqueRanges = new Dictionary<string, string>()
        {
            {"Life Cycle Status", "AC1:AJ1" }, {"Foundation Load Data", "U4:AB4" }
        };

        private static readonly Dictionary<string, string> _headerBorderRanges = new Dictionary<string, string>()
        {
            {"Document Section", "A1:T4" }, {"Approval Section", "U1:AB4" }, {"Revision Section", "AC1:AJ4" }
        };

        private static readonly IReadOnlyList<string> _colonsRanges = new List<string>() { "F1", "F2", "F3", "F4", "Y1", "Y2", "Y3", "AF2", "AF3", "AF4" };


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

        public static Workbook Create(string savePath, string fileName, FLDReport fldReport = null)
        {
            Application excelApp = new Application { Visible = true };

            Workbook workbook = excelApp.Workbooks.Add();
            workbook.SaveAs(Path.Combine(savePath, fileName), XlFileFormat.xlOpenXMLWorkbookMacroEnabled);

            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            worksheet.Name = "Exported Data";

            CreateHeader(worksheet.SetTemplateDefaults());


            /* Generate foundation load data tables from fldReport */

            if (fldReport != null)
            {
                int currentRow = 6;

                FoundationLoadData fld = fldReport.Content as FoundationLoadData;
                HashSet<LoadCaseForces> overallSummary = fld.OverallSummary;

                worksheet.Range($"A{currentRow}").AlignLeftIndented(1).FontStyle("Tahoma", 9, true, false).Fill("Summary of Loads from All Supports (Statics Check):");

                foreach (LoadCaseForces sItem in overallSummary)
                {

                }
            }

            string rngTarget = "A10";
            worksheet.Range(rngTarget).MergeEx().Fill("This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content.").Wrap(15);
















            workbook.Save();

            return workbook;
        }




        public static void CreateHeader(Worksheet worksheet)
        {
            //Dictionary<string, string> rangeCaptionPairs = new Dictionary<string, string>()
            //{
            //    { "A1", "Document No."}, { "A2", "Title"}, { "A3", "Project"}, { "A4", "Client"},
            //    { "U1", "Originator"}, { "U2", "Checker"}, { "U3", "Approver"},{ "U4", "<Template Name>"},
            //    { "AC1", "Life Cycle Status"},
            //    { "AC2", "Code"}, { "AC3", "Revision"}, { "AC4", "Date"}
            //};

            //Dictionary<string, string> rangeValuePairs = new Dictionary<string, string>()
            //{
            //    { "G1", "<Document No.>"}, { "G2", "<Title>"}, { "G3", "<Project>"}, { "G4", "<Client>"},
            //    { "Z1", "<Originator>"}, { "Z2", "<Checker>"}, { "Z3", "<Approver>"},
            //    { "AG2", "<Code>"}, { "AG3", "<Revision>"}, { "AG4", DateTime.Now.ToString("dd-MMM-yyyy")}
            //};

            //HashSet<string> colonsRanges = new HashSet<string>() { "F1", "F2", "F3", "F4", "Y1", "Y2", "Y3", "AF2", "AF3", "AF4" };

            _ = worksheet
                         //.Merge(new HashSet<string>() { "Z1:AB1", "Z2:AB2", "Z3:AB3", "U4:AB4", "AC1:AJ1", "AC2:AE2", "AC3:AE3", "AC4:AE4", "AG2:AJ2", "AG3:AJ3", "AG4:AJ4" })
                         //.Merge(new HashSet<string>() { "A1:E1", "A2:E2", "A3:E3", "A4:E4", "G1:T1", "G2:T2", "G3:T3", "G4:T4", "U1:X1", "U2:X2", "U3:X3" })
                         //.Fill(rangeCaptionPairs)
                         //.Fill(rangeValuePairs)
                         //.Fill(colonsRanges, ":").AlignCenter(colonsRanges)
                         //.AlignLeftIndented(new HashSet<string>() { "A1", "A2", "A3", "A4", "U1", "U2", "U3", "AC2", "AC3", "AC4" }, 1)
                         //.AlignCenter(new HashSet<string>() { "Z1", "Z2", "Z3", "U4", "AC1", "AG2", "AG3", "AG4" })
                         //.BordersAroundAndInsideHorizontal(new HashSet<string>() { "A1:T4", "U1:AB4", "AC1:AJ4" })
                         .SetNumberFormat("AG4", "dd-MM-yyyy");
            //.SetBackgroundColor(new HashSet<string>() { "U4:AB4", "AC1:AJ1" }, _highlight);

            _captionRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill(kvp.Key).MergeEx().AlignLeftIndented(1));
            _valueRangesLeftIndented.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill($"<{kvp.Key}>").MergeEx().AlignLeftIndented(0));
            _valueRangesCenter.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill($"<{kvp.Key}>").MergeEx().AlignCenter());
            _colonsRanges.ToList().ForEach(r => worksheet.Range(r).Fill(":").AlignCenter());
            _uniqueRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).MergeEx().AlignCenter().Fill(kvp.Key).SetBackgroundColor(_highlight).FontStyle(isBold: true));
            _headerBorderRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).BordersAroundAndInsideHorizontal());

        }

        private static HashSet<string> ToHashSet(params string[] items)
        {
            HashSet<string> hashSet = new HashSet<string>();
            foreach (var item in items)
            {
                hashSet.Add(item);
            }
            return hashSet;
        }
    }
}
