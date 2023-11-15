using ReInvented.DataAccess.Services;

namespace ReInvented.Domain.Reporting.Services
{
    public class ReportFileNames
    {
        public static string HtmlFoundationLoadData = $"{FileNames.FoundationLoadDataReport}.{FileExtensions.Html}";

        public static string HtmlMTO = $"{FileNames.MtoReport}.{FileExtensions.Html}";

        public static string CssCommon = $"common.{FileExtensions.Css}";

        public static string CssFoundationLoadData = $"{FileNames.FoundationLoadDataReport}.{FileExtensions.Css}";

        public static string CssMTO = $"{FileNames.MtoReport}.{FileExtensions.Css}";

        public static string JavaScriptCanvasGraphics = $"canvas-graphics.{FileExtensions.Javascript}";
 
        public static string JavaScriptSupportLayoutHelpers = $"support-layout-helpers.{FileExtensions.Javascript}";

        public static string JavaScriptFoundationLoadData = $"{FileNames.FoundationLoadDataReport}.{FileExtensions.Javascript}";

        public static string JavaScriptMTO = $"{FileNames.MtoReport}.{FileExtensions.Javascript}";

        public static string JavaScriptDocumentDataProcessor = $"{FileNames.DocumentDataProcessor}.{FileExtensions.Javascript}";

        public static string ContentsFoundationLoadData = $"{FileNames.FoundationLoadDataReport}-contents.{FileExtensions.Javascript}";

        public static string ContentsMTO = $"{FileNames.MtoReport}-contents.{FileExtensions.Javascript}";

    }
}
