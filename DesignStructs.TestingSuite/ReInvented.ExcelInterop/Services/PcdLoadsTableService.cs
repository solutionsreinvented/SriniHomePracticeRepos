using Microsoft.Office.Interop.Excel;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Models;
using System;

namespace ReInvented.ExcelInterop.Services
{
    public class PcdLoadsTableService
    {
        public static int GenerateHeaders(Worksheet worksheet, int sRowHeader, int nRowsHeader)
        {
            int eRowHeader = sRowHeader + (nRowsHeader - 1);
            const double charSpacing = 0.8;

            //worksheet.Range($"B{sRowHeader}:R{eRowHeader}").Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignLeftIndented(1);

            //worksheet.Range($"S{sRowHeader}:AA{sRowHeader}").Fill($"Forces (kN)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AB{sRowHeader}:AJ{sRowHeader}").Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

            //worksheet.Range($"S{eRowHeader}:U{eRowHeader}").Fill($"Fx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"V{eRowHeader}:X{eRowHeader}").Fill($"Fy").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"Y{eRowHeader}:AA{eRowHeader}").Fill($"Fz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AB{eRowHeader}:AD{eRowHeader}").Fill($"Mx").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AE{eRowHeader}:AG{eRowHeader}").Fill($"My").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            //worksheet.Range($"AH{eRowHeader}:AJ{eRowHeader}").Fill($"Mz").Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();

            return eRowHeader;
        }
    }
}
