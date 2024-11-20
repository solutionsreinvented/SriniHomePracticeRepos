using System;

using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Reporting.ExcelInterop.Extensions;
using ReInvented.Reporting.ExcelInterop.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Reporting.ExcelInterop.Services
{
    public class SupportInformationTableService
    {
        public static int Generate(Worksheet worksheet, int currentRow, SupportsInformation supportsInformation)
        {
            int sColTable = XlSettings.StartColTable;
            int eColTable = XlSettings.EndColTable;
            int colSpanValues = 6 * XlSettings.ColSpanNormal;
            string cgCoordinates = $"( X: {supportsInformation.Center.X:N3}, Y: {supportsInformation.Center.Y:N3}, Z: {supportsInformation.Center.Z:N3} )";
            Releases releases = supportsInformation.SupportReleases;

            int sRow = ++currentRow;
            int eRow = sRow + 4;

            worksheet.Range(currentRow, currentRow, sColTable, eColTable - colSpanValues)
                     .Fill("Support Group C.G.").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);
            worksheet.Range(currentRow, currentRow, eColTable - (colSpanValues - 1), eColTable)
                     .Fill($"{cgCoordinates}").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);

            currentRow++;

            worksheet.Range(currentRow, currentRow, sColTable, eColTable - colSpanValues)
                     .Fill("Diameter from Thickener Center").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);
            worksheet.Range(currentRow, currentRow, eColTable - (colSpanValues - 1), eColTable)
                     .Fill($"{supportsInformation.Diameter:N3} m").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);

            currentRow++;

            worksheet.Range(currentRow, currentRow, sColTable, eColTable - colSpanValues)
                     .Fill("Number of Supports").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);
            worksheet.Range(currentRow, currentRow, eColTable - (colSpanValues - 1), eColTable)
                     .Fill($"{supportsInformation.NumberOfSupports}").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);

            currentRow++;


            int captionsRow = currentRow;
            int valuesRow = currentRow + 1;

            FillSupportConstaintsCaptions(worksheet, eColTable, captionsRow);
            FillSupportConstraintsValues(worksheet, eColTable, releases, valuesRow);

            worksheet.Range(currentRow, ++currentRow, sColTable, eColTable - colSpanValues)
                     .Fill("Support Constraints").MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignLeftIndented(1);

            worksheet.Range(sRow, eRow, sColTable, eColTable).BordersAround().BordersInsideAll();


            return currentRow;
        }

        #region Private Helpers

        private static void FillSupportConstraintCell(Worksheet worksheet, int row, int eColTable, string cellData, int itemIndex, bool isCaption = false)
        {
            Range range = worksheet.Range(row, row, eColTable - (itemIndex * XlSettings.ColSpanNormal - 1), eColTable - (itemIndex - 1) * XlSettings.ColSpanNormal)
                                   .Fill(cellData).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            if (isCaption)
            {
                range.Subscript(2, 1);
            }
        }

        private static void FillSupportConstaintsCaptions(Worksheet worksheet, int eColTable, int captionsRow)
        {
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Rz", 1, true);
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Ry", 2, true);
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Rx", 3, true);
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Tz", 4, true);
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Ty", 5, true);
            FillSupportConstraintCell(worksheet, captionsRow, eColTable, "Tx", 6, true);

            //worksheet.Range(captionsRow, captionsRow, eColTable - (1 * XlSettings.ColSpanNormal - 1), eColTable - 0 * XlSettings.ColSpanNormal)
            //         .Fill("Rz").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(captionsRow, captionsRow, eColTable - (2 * XlSettings.ColSpanNormal - 1), eColTable - 1 * XlSettings.ColSpanNormal)
            //         .Fill("Ry").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(captionsRow, captionsRow, eColTable - (3 * XlSettings.ColSpanNormal - 1), eColTable - 2 * XlSettings.ColSpanNormal)
            //         .Fill("Rx").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(captionsRow, captionsRow, eColTable - (4 * XlSettings.ColSpanNormal - 1), eColTable - 3 * XlSettings.ColSpanNormal)
            //         .Fill("Tz").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(captionsRow, captionsRow, eColTable - (5 * XlSettings.ColSpanNormal - 1), eColTable - 4 * XlSettings.ColSpanNormal)
            //         .Fill("Ty").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(captionsRow, captionsRow, eColTable - (6 * XlSettings.ColSpanNormal - 1), eColTable - 5 * XlSettings.ColSpanNormal)
            //         .Fill("Tx").Subscript(2, 1).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
        }

        private static void FillSupportConstraintsValues(Worksheet worksheet, int eColTable, Releases releases, int valuesRow)
        {
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.Mz), 1);
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.My), 2);
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.Mx), 3);
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.Fz), 4);
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.Fy), 5);
            FillSupportConstraintCell(worksheet, valuesRow, eColTable, releases.ConstraintText(releases.Fx), 6);

            //worksheet.Range(valuesRow, valuesRow, eColTable - (1 * XlSettings.ColSpanNormal - 1), eColTable - 0 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.Mz)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(valuesRow, valuesRow, eColTable - (2 * XlSettings.ColSpanNormal - 1), eColTable - 1 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.My)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(valuesRow, valuesRow, eColTable - (3 * XlSettings.ColSpanNormal - 1), eColTable - 2 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.Mx)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(valuesRow, valuesRow, eColTable - (4 * XlSettings.ColSpanNormal - 1), eColTable - 3 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.Fz)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(valuesRow, valuesRow, eColTable - (5 * XlSettings.ColSpanNormal - 1), eColTable - 4 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.Fy)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
            //worksheet.Range(valuesRow, valuesRow, eColTable - (6 * XlSettings.ColSpanNormal - 1), eColTable - 5 * XlSettings.ColSpanNormal)
            //         .Fill(releases.ConstraintText(releases.Fx)).MergeEx().Wrap(XlSettings.RowHeightStandard, false).AlignCenter();
        }

        #endregion
    }
}
