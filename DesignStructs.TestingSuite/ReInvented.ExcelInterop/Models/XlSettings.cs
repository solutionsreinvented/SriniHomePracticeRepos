using Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInterop.Models
{
    public class CharSpacing
    {
        public const double XT = 0.2;
        public const double T = 0.2;
        public const double XXS = 0.3;
        public const double XS = 0.4;
        public const double S = 0.5;
        public const double N = 0.6;
        public const double M = 0.7;
        public const double L = 0.8;
        public const double XL = 0.9;
        public const double XXL = 1.0;
    }

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
