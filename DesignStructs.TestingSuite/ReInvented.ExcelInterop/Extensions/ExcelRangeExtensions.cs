using System;
using System.Drawing;

using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInterop.Models;
using ReInvented.Shared;

namespace ReInvented.ExcelInterop.Extensions
{
    public static class ExcelRangeExtensions
    {
        #region Private Helpers

        private static double GetAdjustedRowHeight(Range range, int rowStandardHeight, double avgCharWidthRatio)
        {
            string rangeValue;
            double fontSize = range.Font.Size;
            double avgCharWidth = fontSize * avgCharWidthRatio;
            double maxCharsInRow = ((double)range.Width / avgCharWidth).Ceiling(1);

            if (range.Value is object[,] array)
            {
                rangeValue = array[1, 1]?.ToString() ?? string.Empty;
            }
            else
            {
                rangeValue = range.Value.ToString();
            }

            int nContentRows = Math.Max(1, (rangeValue.Length / maxCharsInRow).Ceiling(1));
            int nRangeRows = range.Rows.Count;

            return nContentRows * rowStandardHeight / nRangeRows;
        }

        #endregion

        public static Range FontStyle(this Range range, string name = XlSettings.FontName, int size = XlSettings.FontSize, bool isBold = false, bool isItalic = false,
                                      XlUnderlineStyle xlUnderlineStyle = XlUnderlineStyle.xlUnderlineStyleNone)
        {
            range.Font.Name = name;
            range.Font.Size = size;
            range.Font.Bold = isBold;
            range.Font.Italic = isItalic;
            range.Font.Underline = xlUnderlineStyle;

            return range;
        }

        public static Range FontStyle(this Range range, FontSettings fontSettings)
        {
            range.Font.Name = fontSettings.Name;
            range.Font.Size = fontSettings.Size;
            range.Font.Bold = fontSettings.IsBold;
            range.Font.Italic = fontSettings.IsItalic;
            range.Font.Underline = fontSettings.UnderlineStyle;

            return range;
        }

        public static Range MergeEx(this Range range)
        {
            range.Merge();
            return range;
        }

        public static Range Fill(this Range range, string content)
        {
            range.Cells[1, 1] = content;

            //range.Value = content;
            return range;
        }

        public static Range Subscript(this Range range, int sIndex, int length)
        {
            range.Characters[sIndex, length].Font.Subscript = true;
            return range;
        }

        public static Range Wrap(this Range range, int rowStandardHeight, bool adjustRowHeight, double avgCharWidthRatio = 0.4)
        {
            range.WrapText = true;

            if (range.Columns.Count > 1)
            {
                double rangeRowHeight = adjustRowHeight ? GetAdjustedRowHeight(range, rowStandardHeight, avgCharWidthRatio) : rowStandardHeight;
                range.Rows.RowHeight = Math.Min(rangeRowHeight, 409);
            }
            else
            {
                range.Rows.AutoFit();
            }

            return range;
        }

        public static Range Wrap(this Range range, int rowStandardHeight, double avgCharWidthRatio = XlSettings.AvgCharSpacingNormal)
        {
            return Wrap(range, rowStandardHeight, true, avgCharWidthRatio);
        }

        public static Range AlignCenter(this Range range)
        {
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            return range;
        }

        public static Range AlignLeftIndented(this Range range, int indentLevel)
        {
            range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            range.IndentLevel = indentLevel;
            return range;
        }

        public static Range AlignRightIndented(this Range range, int indentLevel)
        {
            range.HorizontalAlignment = XlHAlign.xlHAlignRight;
            range.IndentLevel = indentLevel;
            return range;
        }

        public static Range BordersAround(this Range range)
        {
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            return range;
        }

        public static Range BordersInsideHorizontal(this Range range)
        {
            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            return range;
        }

        public static Range BordersInsideVertical(this Range range)
        {
            range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            return range;
        }

        public static Range BordersInsideAll(this Range range)
        {
            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            return range;
        }

        public static Range BordersAroundAndInsideHorizontal(this Range range)
        {
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            return range;
        }

        public static Range SetBackgroundColor(this Range range, string hexCode)
        {
            range.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(hexCode));
            return range;
        }

        public static Range SetBackgroundColor(this Range range, Color color)
        {
            range.Interior.Color = ColorTranslator.ToOle(color);
            return range;
        }

        public static Range SetNumberFormat(this Range range, string format)
        {
            range.NumberFormat = format;
            return range;
        }
    }
}
