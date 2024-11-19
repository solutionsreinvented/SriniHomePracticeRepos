using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Extensions;
using ReInvented.ExcelInterop.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.ExcelInterop.Services
{
    public class SummaryTableService
    {

        public static int Generate(Worksheet worksheet, int currentRow, int nRowsHeader, HashSet<LoadCaseForces> forcesSummary, IDictionary<int, string> lcIdTitlePairs)
        {
            currentRow += 2;
            int sRowTable = currentRow;
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;

            currentRow = GenerateHeaders(worksheet, currentRow, nRowsHeader);

            foreach (LoadCaseForces lcForces in forcesSummary)
            {
                string lcTitle = lcIdTitlePairs.FirstOrDefault(kvp => kvp.Key == lcForces.Id).Value;
                currentRow++;
                GenerateContentRow(worksheet, currentRow, lcForces.Id, lcTitle, lcForces.Forces);
            }

            int eRowTable = currentRow;
            worksheet.Range(sRowTable, eRowTable, sColTable, eColTable).BordersAround().BordersInsideAll();

            return currentRow;
        }

        public static int GenerateHeaders(Worksheet worksheet, int sRowHeader, int nRowsHeader, bool includeCgs = false)
        {
            const double charSpacing = CharSpacing.L;
            int eRowHeader = sRowHeader + (nRowsHeader - 1);
            int colSpan = XlSettings.ColSpanNormal;
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;

            int eColCurrent = eColTable;

            if (includeCgs)
            {
                eColCurrent = CreateHeadersForLoadCgColumns(worksheet, sRowHeader, eRowHeader, eColCurrent, colSpan, charSpacing);
            }

            eColCurrent = CreateHeadersForMomentsColumns(worksheet, sRowHeader, eRowHeader, eColCurrent, colSpan, charSpacing);
            eColCurrent = CreateHeadersForForcesColumns(worksheet, sRowHeader, eRowHeader, eColCurrent, colSpan, charSpacing);

            worksheet.Range(sRowHeader, eRowHeader, sColTable, eColCurrent)
                     .Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, false, charSpacing).AlignLeftIndented(1);

            return eRowHeader;
        }


        public static void GenerateContentRow(Worksheet worksheet, int rowIndex, int lcId, string lcTitle, Forces forces)
        {
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;
            int colSpan = XlSettings.ColSpanNormal;


            worksheet.Range(rowIndex, rowIndex, eColTable - (1 * colSpan - 1), eColTable - 0 * colSpan).Fill($"{Math.Round(forces.Mz, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, eColTable - (2 * colSpan - 1), eColTable - 1 * colSpan).Fill($"{Math.Round(forces.My, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, eColTable - (3 * colSpan - 1), eColTable - 2 * colSpan).Fill($"{Math.Round(forces.Mx, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, eColTable - (4 * colSpan - 1), eColTable - 3 * colSpan).Fill($"{Math.Round(forces.Fz, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, eColTable - (5 * colSpan - 1), eColTable - 4 * colSpan).Fill($"{Math.Round(forces.Fy, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, eColTable - (6 * colSpan - 1), eColTable - 5 * colSpan).Fill($"{Math.Round(forces.Fx, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
            worksheet.Range(rowIndex, rowIndex, sColTable, eColTable - 6 * colSpan).Fill($"{lcId}: {lcTitle}").MergeEx().Wrap(XlSettings.RowHeightStandard, true, CharSpacing.N).AlignLeftIndented(1);
        }


        #region Private Static Helpers

        private static int CreateHeadersForLoadCgColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Load C.G", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "X", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Y", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Z", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);

            eColCurrent -= 3 * colSpan;
            return eColCurrent;
        }

        private static int CreateHeadersForMomentsColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Moments (kNm)", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "Mx", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "My", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Mz", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);

            eColCurrent -= 3 * colSpan;
            return eColCurrent;
        }

        private static int CreateHeadersForForcesColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Forces (kN)", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "Fx", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Fy", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Fz", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);

            eColCurrent -= 3 * colSpan;
            return eColCurrent;
        }

        private static Range FillHeaderColumn(Worksheet worksheet, int row, string content, double charSpacing, int eColCurrent, int colSpan, int sCounter, int eCounter)
        {
            return worksheet.Range(row, row, eColCurrent - (sCounter * colSpan - 1), eColCurrent - eCounter * colSpan).Fill(content)
                            .FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
        }

        #endregion

    }
}
