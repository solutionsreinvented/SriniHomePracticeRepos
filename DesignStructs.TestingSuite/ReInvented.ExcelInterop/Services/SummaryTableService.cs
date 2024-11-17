using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.ExcelInterop.Services
{
    public class SummaryTableService
    {

        public static int Generate(Worksheet worksheet, int currentRow, int nRowsHeader, HashSet<LoadCaseForces> forcesSummary, IDictionary<int, string> lcIdTitlePairs)
        {
            currentRow += 2;
            int tableStartRow = currentRow;

            currentRow = GenerateHeaders(worksheet, currentRow, nRowsHeader);

            foreach (LoadCaseForces lcForces in forcesSummary)
            {
                string lcTitle = lcIdTitlePairs.FirstOrDefault(kvp => kvp.Key == lcForces.Id).Value;
                currentRow++;
                GenerateContentRow(worksheet, currentRow, lcForces.Id, lcTitle, lcForces.Forces);
            }

            int tableEndRow = currentRow;
            worksheet.Range($"B{tableStartRow}:AJ{tableEndRow}").BordersAround().BordersInsideAll();

            return currentRow;
        }

        public static int GenerateHeaders(Worksheet worksheet, int sRowHeader, int nRowsHeader)
        {
            int eRowHeader = sRowHeader + (nRowsHeader - 1);
            const double charSpacing = 0.8;

            worksheet.Range($"B{sRowHeader}:R{eRowHeader}").Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignLeftIndented(1);

            worksheet.Range($"S{sRowHeader}:AA{sRowHeader}").Fill($"Forces (kN)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AB{sRowHeader}:AJ{sRowHeader}").Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

            worksheet.Range($"S{eRowHeader}:U{eRowHeader}").Fill($"Fx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"V{eRowHeader}:X{eRowHeader}").Fill($"Fy").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"Y{eRowHeader}:AA{eRowHeader}").Fill($"Fz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AB{eRowHeader}:AD{eRowHeader}").Fill($"Mx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AE{eRowHeader}:AG{eRowHeader}").Fill($"My").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AH{eRowHeader}:AJ{eRowHeader}").Fill($"Mz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

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
            worksheet.Range($"B{rowIndex}:R{rowIndex}").Fill($"{lcId}: {lcTitle}").MergeEx().Wrap(15).AlignLeftIndented(1);
            worksheet.Range($"S{rowIndex}:U{rowIndex}").Fill($"{Math.Round(forces.Fx, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"V{rowIndex}:X{rowIndex}").Fill($"{Math.Round(forces.Fy, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"Y{rowIndex}:AA{rowIndex}").Fill($"{Math.Round(forces.Fz, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AB{rowIndex}:AD{rowIndex}").Fill($"{Math.Round(forces.Mx, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AE{rowIndex}:AG{rowIndex}").Fill($"{Math.Round(forces.My, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AH{rowIndex}:AJ{rowIndex}").Fill($"{Math.Round(forces.Mz, 1)}").MergeEx().Wrap(15).AlignCenter();
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
