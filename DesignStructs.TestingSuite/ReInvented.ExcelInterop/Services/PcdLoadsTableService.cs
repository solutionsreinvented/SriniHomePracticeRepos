using Microsoft.Office.Interop.Excel;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.ExcelInterop.Services
{
    public class PcdLoadsTableService
    {
        public static int GenerateHeaders(Worksheet worksheet, int sRowHeader, Releases releases)
        {
            int nRestrainedTranslations = releases.NumberOfTranslationsRestrained();
            int nRestrainedRotations = releases.NumberOfRotationsRestrained();
            int rowSpanHeader = nRestrainedTranslations > 1 || nRestrainedRotations > 1 ? 2 : 1;
            int colSpanMoments = nRestrainedRotations > 1 ? 3 : 4;
            int colSpanForces = nRestrainedTranslations > 1 ? 3 : 4;


            int sColTable = 2;
            int eColTable = 36;
            int eColCurrent = eColTable;

            int eRowHeader = sRowHeader + (rowSpanHeader - 1);
            const double charSpacing = 0.8;

            /* Moments */

            int totalColSpanMomentsHeader = nRestrainedRotations * colSpanMoments;

            if (nRestrainedRotations > 1)
            {
                worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (totalColSpanMomentsHeader - 1), eColCurrent).Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
            }

            if (!releases.Mz)
            {
                string content = nRestrainedRotations > 1 ? "Mz" : "Mz (kNm)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanMoments;
            }

            if (!releases.My)
            {
                string content = nRestrainedRotations > 1 ? "My" : "My (kNm)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanMoments;
            }

            if (!releases.Mx)
            {
                string content = nRestrainedRotations > 1 ? "Mx" : "Mx (kNm)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanMoments;
            }


            /* Forces */

            int totalColSpanForcesHeader = nRestrainedTranslations * colSpanForces;

            if (nRestrainedTranslations > 1)
            {
                worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (totalColSpanForcesHeader - 1), eColCurrent).Fill($"Forces (kN)").FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();


            }

            if (!releases.Fz)
            {
                string content = nRestrainedTranslations > 1 ? "Fz" : "Fz (kN)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanForces;
            }

            if (!releases.Fy)
            {
                string content = nRestrainedTranslations > 1 ? "Fy" : "Fy (kN)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanForces;
            }

            if (!releases.Fx)
            {
                string content = nRestrainedTranslations > 1 ? "Fx" : "Fx (kN)";
                worksheet.Range(eRowHeader, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent).Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(15, charSpacing).AlignCenter();
                eColCurrent -= colSpanForces;
            }


            worksheet.Range(sRowHeader, eRowHeader, sColTable, eColCurrent).Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(15, false, charSpacing).AlignLeftIndented(1);

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
