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
            //worksheet.Range($"B{sRowTable}:AJ{eRowTable}").BordersAround().BordersInsideAll();
            worksheet.Range(sRowTable, eRowTable, sColTable, eColTable).BordersAround().BordersInsideAll();

            return currentRow;
        }

        private static Range FillHeaderColumn(Worksheet worksheet, int row, string content, double charSpacing, int eColCurrent, int colSpan, int sCounter, int eCounter)
        {
            return worksheet.Range(row, row, eColCurrent - (sCounter * colSpan - 1), eColCurrent - eCounter * colSpan).Fill(content)
                            .FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
        }

        private static int CreateHeadersForLoadCgColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Load C.G", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "X", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Y", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Z", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);

            //worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 0 * colSpan)
            //         .Fill($"Load C.G.").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (1 * colSpan - 1), eColCurrent - 0 * colSpan).Fill($"Z").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (2 * colSpan - 1), eColCurrent - 1 * colSpan).Fill($"Y").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 2 * colSpan).Fill($"X").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();

            eColCurrent -= 3 * colSpan;

            return eColCurrent;
        }

        private static int CreateHeadersForMomentsColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Moments (kNm)", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "Mx", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "My", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Mz", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);


            worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 0 * colSpan)
                     .Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (1 * colSpan - 1), eColCurrent - 0 * colSpan).Fill($"Mz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (2 * colSpan - 1), eColCurrent - 1 * colSpan).Fill($"My").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 2 * colSpan).Fill($"Mx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 0 * colSpan)
            //         .Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (1 * colSpan - 1), eColCurrent - 0 * colSpan).Fill($"Mz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (2 * colSpan - 1), eColCurrent - 1 * colSpan).Fill($"My").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 2 * colSpan).Fill($"Mx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();

            eColCurrent -= 3 * colSpan;

            return eColCurrent;
        }

        private static int CreateHeadersForForcesColumns(Worksheet worksheet, int sRowHeader, int eRowHeader, int eColCurrent, int colSpan, double charSpacing)
        {
            FillHeaderColumn(worksheet, sRowHeader, "Forces (kN)", charSpacing, eColCurrent, colSpan, 3, 0);
            FillHeaderColumn(worksheet, eRowHeader, "Fx", charSpacing, eColCurrent, colSpan, 3, 2).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Fy", charSpacing, eColCurrent, colSpan, 2, 1).Subscript(2, 1);
            FillHeaderColumn(worksheet, eRowHeader, "Fz", charSpacing, eColCurrent, colSpan, 1, 0).Subscript(2, 1);


            //worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 0 * colSpan)
            //         .Fill($"Forces (kN)").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (1 * colSpan - 1), eColCurrent - 0 * colSpan).Fill($"Fz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (2 * colSpan - 1), eColCurrent - 1 * colSpan).Fill($"Fy").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (3 * colSpan - 1), eColCurrent - 2 * colSpan).Fill($"Fx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();

            eColCurrent -= 3 * colSpan;

            return eColCurrent;
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

        public static int GenerateHeadersWithCgs(Worksheet worksheet, int sRowHeader, int nRowsHeader)
        {
            int eRowHeader = sRowHeader + (nRowsHeader - 1);
            //const double charSpacing = 0.8;

            //worksheet.Range($"B{sRowHeader}:R{eRowHeader}").Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignLeftIndented(1);
            //worksheet.Range($"S{sRowHeader}:U{eRowHeader}").Fill($"Fx (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"V{sRowHeader}:X{eRowHeader}").Fill($"Fy (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"Y{sRowHeader}:AA{eRowHeader}").Fill($"Fz (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AB{sRowHeader}:AD{eRowHeader}").Fill($"Mx (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AE{sRowHeader}:AG{eRowHeader}").Fill($"My (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AH{sRowHeader}:AJ{eRowHeader}").Fill($"Mz (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

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

        public static void GenerateContentRowWithCgs(Worksheet worksheet, int rowIndex, int lcId, string lcTitle, Forces forces)
        {
            //worksheet.Range($"B{rowIndex}:R{rowIndex}").Fill($"{lcId} : {lcTitle}").MergeEx().Wrap(15).AlignLeftIndented(1);
            //worksheet.Range($"S{rowIndex}:U{rowIndex}").Fill($"{Math.Round(forces.Fx, 1)}").MergeEx().Wrap(15).AlignCenter();
            //worksheet.Range($"V{rowIndex}:X{rowIndex}").Fill($"{Math.Round(forces.Fy, 1)}").MergeEx().Wrap(15).AlignCenter();
            //worksheet.Range($"Y{rowIndex}:AA{rowIndex}").Fill($"{Math.Round(forces.Fz, 1)}").MergeEx().Wrap(15).AlignCenter();
            //worksheet.Range($"AB{rowIndex}:AD{rowIndex}").Fill($"{Math.Round(forces.Mx, 1)}").MergeEx().Wrap(15).AlignCenter();
            //worksheet.Range($"AE{rowIndex}:AG{rowIndex}").Fill($"{Math.Round(forces.My, 1)}").MergeEx().Wrap(15).AlignCenter();
            //worksheet.Range($"AH{rowIndex}:AJ{rowIndex}").Fill($"{Math.Round(forces.Mz, 1)}").MergeEx().Wrap(15).AlignCenter();
        }

    }
}
