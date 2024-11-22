using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.Shared;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReInvented.Reporting.ExcelInterop.Services
{
    public class DocumentHeaderService
    {
        #region Private Static Fields

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

        public static void CreateDocumentHeader(Worksheet worksheet, string highlightColor)
        {
            _captionRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill(kvp.Key).MergeEx().AlignLeftIndented(1));
            _valueRangesLeftIndented.ToList().ForEach(kvp => worksheet.Range(kvp.Value).MergeEx().AlignLeftIndented(0));
            _valueRangesCenter.ToList().ForEach(kvp => worksheet.Range(kvp.Value).Fill($"<{kvp.Key}>").MergeEx().AlignCenter());
            _colonsRanges.ToList().ForEach(r => worksheet.Range(r).Fill(":").AlignCenter());
            _uniqueRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).MergeEx().AlignCenter().Fill(kvp.Key).SetBackgroundColor(highlightColor).FontStyle(isBold: true));
            _headerBorderRanges.ToList().ForEach(kvp => worksheet.Range(kvp.Value).BordersAroundAndInsideHorizontal());
        }

        public static void FillDocumentHeaderData(Worksheet worksheet, IProjectData projectData, IDocument document)
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
            worksheet.Range(_valueRangesCenter["Date"]).Fill(DateTime.Today.ToShortDateString());

        }
    }
}
