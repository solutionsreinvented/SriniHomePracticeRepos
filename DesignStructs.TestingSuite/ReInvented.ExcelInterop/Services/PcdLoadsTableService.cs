using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.Reporting.Models;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.ExcelInterop.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.ExcelInterop.Services
{
    public class PcdLoadsTableService
    {
        #region Private Static Helpers
        private static int FillLoadColumn(Worksheet worksheet, bool dofReleased, int row, int eColCurrent, int colSpan, double loadValue)
        {
            if (!dofReleased)
            {
                worksheet.Range(row, row, eColCurrent - (colSpan - 1), eColCurrent)
                         .Fill($"{Math.Round(loadValue, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                eColCurrent -= colSpan;
            }
            return eColCurrent;
        }

        private static int FillLoadHeaderColumn(Worksheet worksheet, bool dofReleased, int nRelatedRestraints, int sRowHeader, int eRowHeader,
                                                int eColCurrent, int colSpan, double charSpacing, string headerText, string headerTextUnit)
        {
            if (!dofReleased)
            {
                string content = nRelatedRestraints > 1 ? $"{headerText}" : $"{headerText} ({headerTextUnit})";
                int sRow = nRelatedRestraints > 1 ? eRowHeader : sRowHeader;
                worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpan - 1), eColCurrent)
                         .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
                eColCurrent -= colSpan;
            }

            return eColCurrent;
        } 
        #endregion

        public static int GenerateSupportLoadsRow(Worksheet worksheet, int currentRow, Releases releases, SupportLoads sLoads, IDictionary<int, string> loadCases)
        {
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;

            int nRestrainedTranslations = releases.NumberOfTranslationsRestrained();
            int nRestrainedRotations = releases.NumberOfRotationsRestrained();
            int rowSpanHeader = nRestrainedTranslations > 1 || nRestrainedRotations > 1 ? 2 : 1;
            int colSpanMoments = nRestrainedRotations > 1 ? XlSettings.ColSpanNormal : XlSettings.ColSpanWide;
            int colSpanForces = nRestrainedTranslations > 1 ? XlSettings.ColSpanNormal : XlSettings.ColSpanWide;

            currentRow++;

            int nLoadCases = sLoads.Loads.Count;
            int sRow = currentRow;
            int eRow = currentRow + (nLoadCases - 1);


            worksheet.Range(currentRow, eRow, sColTable, sColTable + (XlSettings.ColSpanNormal - 1))
                     .Fill(sLoads.Support.Id.ToString()).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();

            foreach (LoadCaseForces lc in sLoads.Loads)
            {
                int eColCurrent = eColTable;

                eColCurrent = FillLoadColumn(worksheet, releases.Mz, currentRow, eColCurrent, colSpanMoments, lc.Forces.Mz);
                eColCurrent = FillLoadColumn(worksheet, releases.My, currentRow, eColCurrent, colSpanMoments, lc.Forces.My);
                eColCurrent = FillLoadColumn(worksheet, releases.Mx, currentRow, eColCurrent, colSpanMoments, lc.Forces.Mx);

                eColCurrent = FillLoadColumn(worksheet, releases.Fz, currentRow, eColCurrent, colSpanForces, lc.Forces.Fz);
                eColCurrent = FillLoadColumn(worksheet, releases.Fy, currentRow, eColCurrent, colSpanForces, lc.Forces.Fy);
                eColCurrent = FillLoadColumn(worksheet, releases.Fx, currentRow, eColCurrent, colSpanForces, lc.Forces.Fx);

                //if (!releases.Mz)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanMoments - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.Mz, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanMoments;
                //}
                //if (!releases.My)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanMoments - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.My, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanMoments;
                //}
                //if (!releases.Mx)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanMoments - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.Mx, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanMoments;
                //}
                //if (!releases.Fz)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanForces - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.Fz, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanForces;
                //}
                //if (!releases.Fy)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanForces - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.Fy, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanForces;
                //}
                //if (!releases.Fx)
                //{
                //    worksheet.Range(currentRow, currentRow, eColCurrent - (colSpanForces - 1), eColCurrent)
                //             .Fill($"{Math.Round(lc.Forces.Fx, 1)}").MergeEx().Wrap(XlSettings.RowHeightStandard).AlignCenter();
                //    eColCurrent -= colSpanForces;
                //}

                worksheet.Range(currentRow, currentRow, sColTable + 3, eColCurrent)
                         .Fill($"{lc.Id}: {loadCases.FirstOrDefault(c => c.Key == lc.Id).Value}").MergeEx().Wrap(XlSettings.RowHeightStandard, true, CharSpacing.N).AlignLeftIndented(1);

                currentRow++;

            }

            worksheet.Range(sRow, eRow, sColTable, eColTable).BordersAround().BordersInsideAll();

            return eRow;
        }



        public static int GenerateHeaders(Worksheet worksheet, int sRowHeader, Releases releases)
        {
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;
            const double charSpacing = XlSettings.AvgCharSpacingHeaders;

            int nRestrainedTranslations = releases.NumberOfTranslationsRestrained();
            int nRestrainedRotations = releases.NumberOfRotationsRestrained();
            int rowSpanHeader = nRestrainedTranslations > 1 || nRestrainedRotations > 1 ? 2 : 1;
            int colSpanMoments = nRestrainedRotations > 1 ? XlSettings.ColSpanNormal : XlSettings.ColSpanWide;
            int colSpanForces = nRestrainedTranslations > 1 ? XlSettings.ColSpanNormal : XlSettings.ColSpanWide;


            int eColCurrent = eColTable;
            int eRowHeader = sRowHeader + (rowSpanHeader - 1);

            /* Moments */

            int totalColSpanMomentsHeader = nRestrainedRotations * colSpanMoments;

            if (nRestrainedRotations > 1)
            {
                worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (totalColSpanMomentsHeader - 1), eColCurrent)
                         .Fill($"Moments (kNm)").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            }


            eColCurrent = FillLoadHeaderColumn(worksheet, releases.Mz, nRestrainedRotations, sRowHeader, eRowHeader, eColCurrent, colSpanMoments, charSpacing, "Mz", "kNm");
            eColCurrent = FillLoadHeaderColumn(worksheet, releases.My, nRestrainedRotations, sRowHeader, eRowHeader, eColCurrent, colSpanMoments, charSpacing, "My", "kNm");
            eColCurrent = FillLoadHeaderColumn(worksheet, releases.Mx, nRestrainedRotations, sRowHeader, eRowHeader, eColCurrent, colSpanMoments, charSpacing, "Mx", "kNm");

            //if (!releases.Mz)
            //{
            //    string content = nRestrainedRotations > 1 ? "Mz" : "Mz (kNm)";
            //    int sRow = nRestrainedRotations > 1 ? eRowHeader : sRowHeader;
            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanMoments;
            //}

            //if (!releases.My)
            //{
            //    string content = nRestrainedRotations > 1 ? "My" : "My (kNm)";
            //    int sRow = nRestrainedRotations > 1 ? eRowHeader : sRowHeader;
            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanMoments;
            //}

            //if (!releases.Mx)
            //{
            //    string content = nRestrainedRotations > 1 ? "Mx" : "Mx (kNm)";
            //    int sRow = nRestrainedRotations > 1 ? eRowHeader : sRowHeader;
            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanMoments - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanMoments;
            //}


            /* Forces */

            int totalColSpanForcesHeader = nRestrainedTranslations * colSpanForces;

            if (nRestrainedTranslations > 1)
            {
                worksheet.Range(sRowHeader, sRowHeader, eColCurrent - (totalColSpanForcesHeader - 1), eColCurrent)
                         .Fill($"Forces (kN)").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            }

            eColCurrent = FillLoadHeaderColumn(worksheet, releases.Fz, nRestrainedTranslations, sRowHeader, eRowHeader, eColCurrent, colSpanForces, charSpacing, "Fz", "kN");
            eColCurrent = FillLoadHeaderColumn(worksheet, releases.Fy, nRestrainedTranslations, sRowHeader, eRowHeader, eColCurrent, colSpanForces, charSpacing, "Fy", "kN");
            eColCurrent = FillLoadHeaderColumn(worksheet, releases.Fx, nRestrainedTranslations, sRowHeader, eRowHeader, eColCurrent, colSpanForces, charSpacing, "Fx", "kN");

            //if (!releases.Fz)
            //{
            //    string content = nRestrainedTranslations > 1 ? "Fz" : "Fz (kN)";
            //    int sRow = nRestrainedTranslations > 1 ? eRowHeader : sRowHeader;

            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanForces;
            //}

            //if (!releases.Fy)
            //{
            //    string content = nRestrainedTranslations > 1 ? "Fy" : "Fy (kN)";
            //    int sRow = nRestrainedTranslations > 1 ? eRowHeader : sRowHeader;

            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanForces;
            //}

            //if (!releases.Fx)
            //{
            //    string content = nRestrainedTranslations > 1 ? "Fx" : "Fx (kN)";
            //    int sRow = nRestrainedTranslations > 1 ? eRowHeader : sRowHeader;

            //    worksheet.Range(sRow, eRowHeader, eColCurrent - (colSpanForces - 1), eColCurrent)
            //             .Fill(content).Subscript(2, 1).FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, charSpacing).AlignCenter();
            //    eColCurrent -= colSpanForces;
            //}


            worksheet.Range(sRowHeader, eRowHeader, sColTable + XlSettings.ColSpanNormal, eColCurrent)
                     .Fill($"Load Case/Combination").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, false, charSpacing).AlignLeftIndented(1);
            worksheet.Range(sRowHeader, eRowHeader, sColTable, sColTable + (XlSettings.ColSpanNormal - 1))
                     .Fill($"Node").FontStyle(isBold: true).MergeEx().Wrap(XlSettings.RowHeightStandard, false, charSpacing).AlignCenter();
            worksheet.Range(sRowHeader, eRowHeader, sColTable, eColTable).BordersAround().BordersInsideAll();

            return eRowHeader;
        }

    }
}
