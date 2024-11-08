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
using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.Domain.ProjectSetup.Interfaces;

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

        private static void FillDocumentData(Worksheet worksheet, IProjectData projectData, IDocument document)
        {
            worksheet.Range(_valueRangesLeftIndented["Document No."]).Fill(document.Number);
            worksheet.Range(_valueRangesLeftIndented["Title"]).Fill(document.Title);
            worksheet.Range(_valueRangesLeftIndented["Project"]).Fill(projectData.Name);
            worksheet.Range(_valueRangesLeftIndented["Client"]).Fill(projectData.Client);

            IRevision lastRev = document.Revisions.LastOrDefault();
            IScrutinyHistory scrutiny = lastRev.ScrutinyHistory;

            worksheet.Range(_valueRangesCenter["Originator"]).Fill(scrutiny.Originator.ShortName);
            worksheet.Range(_valueRangesCenter["Checker"]).Fill(scrutiny.Reviewer.ShortName);
            worksheet.Range(_valueRangesCenter["Approver"]).Fill(scrutiny.Approver.ShortName);

            worksheet.Range(_valueRangesCenter["Code"]).Fill(lastRev.SubmissionCategory.GetReleaseCode());
            worksheet.Range(_valueRangesCenter["Revision"]).Fill(lastRev.Code.ToString());
            worksheet.Range(_valueRangesCenter["Date"]).Fill(DateTime.Today.ToString()).SetNumberFormat("dd-MM-yyyy");

        }

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

                FillDocumentData(worksheet, fldReport.ProjectData, fldReport.Document);
                worksheet.Range($"A{currentRow}").AlignLeftIndented(1).FontStyle("Tahoma", 9, true, false).Fill("Summary of Loads from All Supports (Statics Check):");
                foreach (LoadCaseForces sItem in overallSummary)
                {
                    worksheet.Application.ActiveWindow.DisplayGridlines = false;
                }
            }

            string rngTarget = "A10";
            worksheet.Range(rngTarget).MergeEx().Fill("This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content.").Wrap(15);
















            workbook.Save();

            return workbook;
        }




        public static void CreateHeader(Worksheet worksheet)
        {
            _captionRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill(kvp.Key).MergeEx().AlignLeftIndented(1));
            _valueRangesLeftIndented.ToList().ForEach(kvp => worksheet.Range(kvp.Value).MergeEx().AlignLeftIndented(0));
            _valueRangesCenter.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill($"<{kvp.Key}>").MergeEx().AlignCenter());
            _colonsRanges.ToList().ForEach(r => worksheet.Range(r).Fill(":").AlignCenter());
            _uniqueRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).MergeEx().AlignCenter().Fill(kvp.Key).SetBackgroundColor(_highlight).FontStyle(isBold: true));
            _headerBorderRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).BordersAroundAndInsideHorizontal());
        }

        public static void FillHeaderValues(Worksheet worksheet)
        {
            _ = worksheet.Range("AG4").Fill(DateTime.Today.ToString()).SetNumberFormat("dd-MM-yyyy");
            _ = worksheet.Range("AG4").Fill(DateTime.Today.ToString()).SetNumberFormat("dd-MM-yyyy");
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
