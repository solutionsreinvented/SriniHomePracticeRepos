using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

namespace ReInvented.Domain.Reporting.Services
{
    public class HtmlContentManager
    {

        public static HtmlDocument LinkCssAndScriptsTo(HtmlDocument htmlDocument)
        {
            htmlDocument = AddCssLinkTagsToHeadElement(htmlDocument);
            htmlDocument = AddScriptTagsToBodyElement(htmlDocument);

            return htmlDocument;
        }


        #region Private Helpers

        private static HtmlDocument AddCssLinkTagsToHeadElement(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException($"{nameof(htmlDocument)} cannot be null or empty.");
            }

            HtmlNode head = htmlDocument.DocumentNode.SelectSingleNode("//head");

            if (head != null)
            {
                // Remove all existing <link> elements from the <body>
                HtmlNodeCollection existingStylesheetElements = head.SelectNodes("//link[@rel='stylesheet']");
                if (existingStylesheetElements != null && existingStylesheetElements.Any())
                {
                    foreach (HtmlNode stylesheetElement in existingStylesheetElements.ToList())
                    {
                        stylesheetElement.Remove();
                    }
                }

                // Create and add two new <script> elements
                HtmlNode commonCssNode = GetStylesheetNodeWithAttributes($"Styles/{ReportFileNames.CssCommon}");
                HtmlNode fdlCssNode = GetStylesheetNodeWithAttributes($"Styles/{ReportFileNames.CssFoundationLoadData}");

                _ = head.AppendChild(commonCssNode);
                _ = head.AppendChild(fdlCssNode);
            }

            return htmlDocument;
        }

        private static HtmlDocument AddScriptTagsToBodyElement(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException($"{nameof(htmlDocument)} cannot be null or empty.");
            }

            HtmlNode body = htmlDocument.DocumentNode.SelectSingleNode("//body");

            if (body != null)
            {
                // Remove all existing <script> elements from the <body>
                HtmlNodeCollection scriptElements = body.SelectNodes(".//script");
                if (scriptElements != null && scriptElements.Any())
                {
                    foreach (HtmlNode scriptElement in scriptElements.ToList())
                    {
                        scriptElement.Remove();
                    }
                }

                // Create and add two new <script> elements
                HtmlNode fdlContentsScriptTag = GetScriptNodeWithAttributes($"Data/{ReportFileNames.ContentsFoundationLoadData}");
                _ = body.AppendChild(fdlContentsScriptTag);

                HtmlNode fdlReportGeneratorScriptTag = GetScriptNodeWithAttributes($"Scripts/{ReportFileNames.JavaScriptFoundationLoadData}");
                _ = body.AppendChild(fdlReportGeneratorScriptTag);
            }

            return htmlDocument;
        }

        private static HtmlNode GetStylesheetNodeWithAttributes(string cssFilePath)
        {
            HtmlNode stylesheetNode = HtmlNode.CreateNode("<link></link>");

            _ = stylesheetNode.SetAttributeValue("rel", "stylesheet");
            _ = stylesheetNode.SetAttributeValue("type", "text/css");
            _ = stylesheetNode.SetAttributeValue("href", cssFilePath);

            return stylesheetNode;
        }

        private static HtmlNode GetScriptNodeWithAttributes(string scriptFilePath)
        {
            HtmlNode scriptNode = HtmlNode.CreateNode("<script></script>");
            scriptNode.Attributes.Add("src", scriptFilePath);

            return scriptNode;
        } 

        #endregion


        //public static List<string> AddScriptTagsTo(List<string> originalHtmlContent)
        //{
        //    List<string> scriptContent = new List<string>()
        //    {
        //        $"<script src=\"./Data/{ReportFileNames.ContentsFoundationLoadData}\"></script>",
        //        $"<script src=\"./Scripts/{ReportFileNames.JavaScriptFoundationLoadData}\"></script>"
        //    };
        //    return AddScriptTagsTo(originalHtmlContent, scriptContent);
        //}

        //public static List<string> AddScriptTagsTo(List<string> originalHtmlContent, List<string> scriptContent)
        //{
        //    int bodyClosingTagIndex = originalHtmlContent.IndexOf("</body>");

        //    originalHtmlContent.InsertRange(bodyClosingTagIndex - 1, scriptContent);

        //    return originalHtmlContent;
        //}
    }
}
