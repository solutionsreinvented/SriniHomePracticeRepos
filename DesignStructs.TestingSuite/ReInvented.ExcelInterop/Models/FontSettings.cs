using Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInterop.Models
{
    public class FontSettings
    {
        #region Default Constructor

        public FontSettings()
        {

        }

        #endregion

        #region Parameterized Constructor

        public FontSettings(string name, int size, bool isBold, bool isItalic, XlUnderlineStyle underlineStyle)
        {
            Name = name;
            Size = size;
            IsBold = isBold;
            IsItalic = isItalic;
            UnderlineStyle = underlineStyle;
        }

        #endregion

        public string Name { get; set; }
        public int Size { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public XlUnderlineStyle UnderlineStyle { get; set; }
    }
}
