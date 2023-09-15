using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Models;
using System;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class SummarySheetService
    {
        public static void FillForcesInSummaryTable(Excel.Worksheet wsSummary, List<MemberForces> forcesSummary)
        {
            if (forcesSummary.Count != 12)
            {
                throw new ArgumentException($"Invalid {nameof(forcesSummary)}. The items in the argument shall be exactly 12.");
            }

            Excel.Range rngForcesSummary = wsSummary.Range["InputForcesSummaryTable"];

            int sRow = rngForcesSummary.Row;
            int sColumn = rngForcesSummary.Column;

            for (int iRow = 0; iRow < forcesSummary.Count; iRow++)
            {
                wsSummary.Cells[sRow + iRow, sColumn + 0] = forcesSummary[iRow].MemberId;
                wsSummary.Cells[sRow + iRow, sColumn + 1] = $"{forcesSummary[iRow].LoadCaseId} {forcesSummary[iRow].LoadCaseTitle}";
                ///wsSummary.Cells[sRow + iRow, sColumn + 2] = forcesSummary[iRow].NodeId; /// Add NodeId to the MemberForces class and then uncomment this.
                wsSummary.Cells[sRow + iRow, sColumn + 3] = forcesSummary[iRow].Fx;
                wsSummary.Cells[sRow + iRow, sColumn + 4] = forcesSummary[iRow].Fy;
                wsSummary.Cells[sRow + iRow, sColumn + 5] = forcesSummary[iRow].Fz;
                wsSummary.Cells[sRow + iRow, sColumn + 6] = forcesSummary[iRow].Mx;
                wsSummary.Cells[sRow + iRow, sColumn + 7] = forcesSummary[iRow].My;
                wsSummary.Cells[sRow + iRow, sColumn + 8] = forcesSummary[iRow].Mz;
            }
        }

        public static GoverningCriteria GetGoverningCriteria(Excel.Worksheet wsSummary)
        {
            Excel.Range rngGoverningCriteria = wsSummary.Range["OutputGoverningCriteria"];

            int sRow = rngGoverningCriteria.Row;
            int sColumn = rngGoverningCriteria.Column;

            GoverningCriteria governingCriteria = new GoverningCriteria()
            {
                MemberId = rngGoverningCriteria.Cells[sRow, sColumn + 0],
                LoadCaseId = rngGoverningCriteria.Cells[sRow, sColumn + 1],
                LoadCaseTitle = rngGoverningCriteria.Cells[sRow, sColumn + 2]
            };

            return governingCriteria;
        }
    }
}
