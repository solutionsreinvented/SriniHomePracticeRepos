using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Services
{
    public class HtmlContentManager
    {
        public static List<string> AddScriptTagsTo(List<string> originalHtmlContent)
        {
            List<string> scriptContent = new List<string>()
            {
                $"<script src=\"./Data/{ReportFileNames.ContentsFoundationLoadData}\"></script>",
                $"<script src=\"./Scripts/{ReportFileNames.JavaScriptFoundationLoadData}\"></script>"
            };
            return AddScriptTagsTo(originalHtmlContent, scriptContent);
        }

        public static List<string> AddScriptTagsTo(List<string> originalHtmlContent, List<string> scriptContent)
        {
            int bodyClosingTagIndex = originalHtmlContent.IndexOf("</body>");

            originalHtmlContent.InsertRange(bodyClosingTagIndex - 1, scriptContent);

            return originalHtmlContent;
        }
    }
}
