using System;
using System.Linq;

using HtmlAgilityPack;

using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.Extensions
{
    public static class HtmlDocumentExtensions
    {
        public static HtmlDocument LinkCssAndScriptsTo(this HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException($"{nameof(htmlDocument)} cannot be null or empty.");
            }

            return htmlDocument.RemoveCssLinkTagsFromHeadElement()
                               .AppendCssLinkTagsToHeadElement(useAbsolutePaths)
                               .RemoveScriptTagsFromBodyElement()
                               .AppendScriptTagsToBodyElement(useAbsolutePaths);
        }

        public static HtmlDocument RemoveCssLinkTagsFromHeadElement(this HtmlDocument htmlDocument)
        {
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
            }

            return htmlDocument;
        }

        public static HtmlDocument RemoveScriptTagsFromBodyElement(this HtmlDocument htmlDocument)
        {
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
            }

            return htmlDocument;
        }

        public static HtmlDocument AppendCssLinkTagsToHeadElement(this HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode head = htmlDocument.DocumentNode.SelectSingleNode("//head");

            if (head != null)
            {
                _ = head.AppendChild(HtmlNodesService.GetStylesheetNodeWithAttributes(ReportFileNames.CssCommon, useAbsolutePaths));
                _ = head.AppendChild(HtmlNodesService.GetStylesheetNodeWithAttributes(ReportFileNames.CssFoundationLoadData, useAbsolutePaths));

            }

            return htmlDocument;
        }

        public static HtmlDocument AppendScriptTagsToBodyElement(this HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode body = htmlDocument.DocumentNode.SelectSingleNode("//body");

            if (body != null)
            {
                _ = body.AppendChild(HtmlNodesService.GetScriptNodeWithAttributes($"Data/{ReportFileNames.ContentsFoundationLoadData}"));
                _ = body.AppendChild(HtmlNodesService.GetScriptNodeWithAttributes(ReportFileNames.JavaScriptCanvasGraphics, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodesService.GetScriptNodeWithAttributes(ReportFileNames.JavaScriptSupportLayoutHelpers, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodesService.GetScriptNodeWithAttributes(ReportFileNames.JavaScriptFoundationLoadData, useAbsolutePaths));
            }

            return htmlDocument;
        }
    }
}
