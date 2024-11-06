using System.Drawing;

using Excel = Microsoft.Office.Interop.Excel;


namespace ReInvented.ExcelInterop.Extensions
{
    public static class ExcelRangeExtensions
    {
        public static void AlignCenter(this Excel.Range range)
        {
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        public static void AlignLeftIndented(this Excel.Range range, int indentLevel)
        {
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            range.IndentLevel = indentLevel;
        }

        public static void AlignRightIndented(this Excel.Range range, int indentLevel)
        {
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            range.IndentLevel = indentLevel;
        }

        public static void BordersAroundAndInsideHorizontal(this Excel.Range range)
        {
            range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

            range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
        }

        public static void SetBackgroundColor(this Excel.Range range, string hexCode)
        {
            range.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(hexCode));
        }

        public static void SetBackgroundColor(this Excel.Range range, Color color)
        {
            range.Interior.Color = ColorTranslator.ToOle(color);
        }

        public static void SetNumberFormat(this Excel.Range range, string format)
        {
            range.NumberFormat = format;
        }
    }
}
