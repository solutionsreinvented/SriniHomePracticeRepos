using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInterop.Extensions
{
    public static class WorksheetExtensions
    {
        public static Range Range(this Worksheet worksheet, string range)
        {
            return worksheet.Range[range];
        }

        public static Range Range(this Worksheet worksheet, int sRow, int eRow, int sCol, int eCol)
        {
            return worksheet.Range[worksheet.Cells[sRow, sCol], worksheet.Cells[eRow, eCol]];
        }

        public static Worksheet SetTemplateDefaults(this Worksheet worksheet, int rowHeight, int colWidth, string fontName, int fontSize)
        {
            worksheet.Rows.RowHeight = rowHeight;
            worksheet.Columns.ColumnWidth = colWidth;
            worksheet.Cells.Font.Name = fontName;
            worksheet.Cells.Font.Size = fontSize;
            worksheet.Cells.VerticalAlignment = XlVAlign.xlVAlignCenter;

            return worksheet;
        }

        public static Worksheet SetTemplateDefaults(this Worksheet worksheet)
        {
            return SetTemplateDefaults(worksheet, 15, 2, "Tahoma", 8);
        }

        public static Worksheet Merge(this Worksheet worksheet, HashSet<string> ranges)
        {
            ranges.ToList().ForEach(rng => Merge(worksheet, rng));
            return worksheet;
        }

        public static Worksheet Merge(this Worksheet worksheet, string range)
        {
            worksheet.Range[range].Merge();
            return worksheet;
        }

        public static Worksheet Fill(this Worksheet worksheet, Dictionary<string, string> rangeContentPairs)
        {
            rangeContentPairs.ToList().ForEach(rcp => Fill(worksheet, rcp.Key, rcp.Value));
            return worksheet;
        }

        public static Worksheet Fill(this Worksheet worksheet, HashSet<string> ranges, string content)
        {
            ranges.ToList().ForEach(rng => Fill(worksheet, rng, content));
            return worksheet;
        }

        public static Worksheet Fill(this Worksheet worksheet, string range, string content)
        {
            worksheet.Range[range].Value = content;
            return worksheet;
        }

        public static Worksheet Wrap(this Worksheet worksheet, string range, int rowStandardHeight, double avgCharWidthRatio = 0.4)
        {
            worksheet.Range[range].Wrap(rowStandardHeight, avgCharWidthRatio);
            return worksheet;
        }

        public static Worksheet AlignCenter(this Worksheet worksheet, HashSet<string> ranges)
        {
            ranges.ToList().ForEach(rng => worksheet.Range[rng].AlignCenter());
            return worksheet;
        }

        public static Worksheet AlignLeftIndented(this Worksheet worksheet, HashSet<string> ranges, int indentLevel)
        {
            ranges.ToList().ForEach(rng => worksheet.Range[rng].AlignLeftIndented(indentLevel));
            return worksheet;
        }

        public static Worksheet AlignRightIndented(this Worksheet worksheet, HashSet<string> ranges, int indentLevel)
        {
            ranges.ToList().ForEach(rng => worksheet.Range[rng].AlignLeftIndented(indentLevel));
            return worksheet;

        }

        public static Worksheet SetBackgroundColor(this Worksheet worksheet, HashSet<string> ranges, Color color)
        {
            ranges.ToList().ForEach(rng => SetBackgroundColor(worksheet, rng, color));
            return worksheet;

        }

        public static Worksheet SetBackgroundColor(this Worksheet worksheet, HashSet<string> ranges, string hexCode)
        {
            ranges.ToList().ForEach(rng => SetBackgroundColor(worksheet, rng, hexCode));
            return worksheet;

        }

        public static Worksheet SetBackgroundColor(this Worksheet worksheet, string range, Color color)
        {
            worksheet.Range[range].SetBackgroundColor(color);
            return worksheet;

        }

        public static Worksheet SetBackgroundColor(this Worksheet worksheet, string range, string hexCode)
        {
            worksheet.Range[range].SetBackgroundColor(hexCode);
            return worksheet;

        }

        public static Worksheet SetNumberFormat(this Worksheet worksheet, HashSet<string> ranges, string format)
        {
            ranges.ToList().ForEach(rng => SetNumberFormat(worksheet, rng, format));
            return worksheet;

        }

        public static Worksheet SetNumberFormat(this Worksheet worksheet, string range, string format)
        {
            worksheet.Range[range].SetNumberFormat(format);
            return worksheet;

        }

        public static Worksheet BordersAroundAndInsideHorizontal(this Worksheet worksheet, HashSet<string> ranges)
        {
            ranges.ToList().ForEach(rng => BordersAroundAndInsideHorizontal(worksheet, rng));
            return worksheet;

        }

        public static Worksheet BordersAroundAndInsideHorizontal(this Worksheet worksheet, string range)
        {
            worksheet.Range[range].BordersAroundAndInsideHorizontal();
            return worksheet;

        }

    }
}
