using System;

using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.ExcelInterop.Services
{
    public class OverallSummaryService
    {
        public static int GenerateHeadersForTableWithoutCgs(Worksheet worksheet, int sRowHeader, int nRowsHeader)
        {
            int eRowHeader = sRowHeader + (nRowsHeader - 1);
            const double charSpacing = 0.8;

            worksheet.Range($"B{sRowHeader}:R{eRowHeader}").Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignLeftIndented(1);
            worksheet.Range($"S{sRowHeader}:U{eRowHeader}").Fill($"Fx (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"V{sRowHeader}:X{eRowHeader}").Fill($"Fy (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"Y{sRowHeader}:AA{eRowHeader}").Fill($"Fz (kN)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AB{sRowHeader}:AD{eRowHeader}").Fill($"Mx (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AE{sRowHeader}:AG{eRowHeader}").Fill($"My (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            worksheet.Range($"AH{sRowHeader}:AJ{eRowHeader}").Fill($"Mz (kNm)").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

            return eRowHeader;
        }

        public static void GenerateContentRowForTableWithoutCgs(Worksheet worksheet, int rowIndex, int lcId, string lcTitle, Forces forces)
        {
            worksheet.Range($"B{rowIndex}:R{rowIndex}").Fill($"{lcId} : {lcTitle}").MergeEx().Wrap(15).AlignLeftIndented(1);
            worksheet.Range($"S{rowIndex}:U{rowIndex}").Fill($"{Math.Round(forces.Fx, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"V{rowIndex}:X{rowIndex}").Fill($"{Math.Round(forces.Fy, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"Y{rowIndex}:AA{rowIndex}").Fill($"{Math.Round(forces.Fz, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AB{rowIndex}:AD{rowIndex}").Fill($"{Math.Round(forces.Mx, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AE{rowIndex}:AG{rowIndex}").Fill($"{Math.Round(forces.My, 1)}").MergeEx().Wrap(15).AlignCenter();
            worksheet.Range($"AH{rowIndex}:AJ{rowIndex}").Fill($"{Math.Round(forces.Mz, 1)}").MergeEx().Wrap(15).AlignCenter();
        }

    }
}
