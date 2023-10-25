using System;
using System.IO;
using System.Linq;

using HtmlAgilityPack;

using ReInvented.DataAccess.Services;

namespace ReInvented.Domain.Reporting.Services
{
    public class HtmlContentManager
    {
        #region Public Functions

        public static HtmlDocument LinkCssAndScriptsTo(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            htmlDocument = AddCssLinkTagsToHeadElement(htmlDocument, useAbsolutePaths);
            htmlDocument = AddScriptTagsToBodyElement(htmlDocument, useAbsolutePaths);

            return htmlDocument;
        }

        #endregion

        #region Private Helpers

        private static HtmlDocument AddCssLinkTagsToHeadElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
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

                if (useAbsolutePaths)
                {
                    _ = head.AppendChild(GetStylesheetNodeWithAttributes(FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.StylesDirectory, ReportFileNames.CssCommon)));
                    _ = head.AppendChild(GetStylesheetNodeWithAttributes(FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.StylesDirectory, ReportFileNames.CssFoundationLoadData)));
                }
                else
                {
                    _ = head.AppendChild(GetStylesheetNodeWithAttributes($"Styles/{ReportFileNames.CssCommon}"));
                    _ = head.AppendChild(GetStylesheetNodeWithAttributes($"Styles/{ReportFileNames.CssFoundationLoadData}"));
                }

            }

            return htmlDocument;
        }

        private static HtmlDocument AddScriptTagsToBodyElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
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

                _ = body.AppendChild(GetScriptNodeWithAttributes($"Data/{ReportFileNames.ContentsFoundationLoadData}"));
                if (useAbsolutePaths)
                {
                    _ = body.AppendChild(GetScriptNodeWithAttributes(FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.ScriptsDirectory, ReportFileNames.JavaScriptCanvasGraphics)));
                    _ = body.AppendChild(GetScriptNodeWithAttributes(FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.ScriptsDirectory, ReportFileNames.JavaScriptSupportLayoutHelpers)));
                    _ = body.AppendChild(GetScriptNodeWithAttributes(FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.ScriptsDirectory, ReportFileNames.JavaScriptFoundationLoadData)));
                }
                else
                {
                    _ = body.AppendChild(GetScriptNodeWithAttributes($"Scripts/{ReportFileNames.JavaScriptCanvasGraphics}"));
                    _ = body.AppendChild(GetScriptNodeWithAttributes($"Scripts/{ReportFileNames.JavaScriptSupportLayoutHelpers}"));
                    _ = body.AppendChild(GetScriptNodeWithAttributes($"Scripts/{ReportFileNames.JavaScriptFoundationLoadData}"));
                }
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
    }
}
