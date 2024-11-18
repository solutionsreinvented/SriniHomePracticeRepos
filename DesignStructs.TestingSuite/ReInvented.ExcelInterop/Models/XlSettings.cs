using Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInterop.Models
{
    public class XlSettings
    {
        public const string HeaderBackground = "#F0F8FF";

        public const string FontName = "Tahoma";
        public const int FontSize = 8;

        public const int RowHeightStandard = 15;
        public const int HeadersOffset = 2;
        public const int ContentStartRow = 6;

        public const int StartColTable = 2;
        public const int EndColTable = 35;
        public const int ColSpanNormal = 3;
        public const int ColSpanWide = 4;
        public const double AvgCharSpacingHeaders = CharSpacing.L;
        public const double AvgCharSpacingNormal = CharSpacing.XS;

        public static FontSettings HeaderDefaultFont { get; set; } = new FontSettings(FontName, FontSize, true, false, XlUnderlineStyle.xlUnderlineStyleNone);
        public static FontSettings ContentDefaultFont { get; set; } = new FontSettings(FontName, FontSize, false, false, XlUnderlineStyle.xlUnderlineStyleNone);

    }
}
