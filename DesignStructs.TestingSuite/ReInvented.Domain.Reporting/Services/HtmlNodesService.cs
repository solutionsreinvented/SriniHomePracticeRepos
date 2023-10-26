using HtmlAgilityPack;

using ReInvented.DataAccess.Services;

namespace ReInvented.Domain.Reporting.Services
{
    public static class HtmlNodesService
    {
        public static HtmlNode GetStylesheetNodeWithAttributes(string cssFilename, bool useAbsolutePaths)
        {
            string cssFilePath = useAbsolutePaths ? FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.StylesDirectory, cssFilename) : $"Scripts/{cssFilename}";

            HtmlNode stylesheetNode = HtmlNode.CreateNode("<link></link>");

            _ = stylesheetNode.SetAttributeValue("rel", "stylesheet");
            _ = stylesheetNode.SetAttributeValue("type", "text/css");
            _ = stylesheetNode.SetAttributeValue("href", cssFilePath);

            return stylesheetNode;
        }

        public static HtmlNode GetScriptNodeWithAttributes(string scriptFilePath)
        {
            HtmlNode scriptNode = HtmlNode.CreateNode("<script></script>");
            scriptNode.Attributes.Add("src", scriptFilePath);

            return scriptNode;
        }

        public static HtmlNode GetScriptNodeWithAttributes(string scriptFilename, bool useAbsolutePaths)
        {
            string scriptFilePath = useAbsolutePaths ? FileServiceProvider.GetAbsolutePathInWebFormat(FileServiceProvider.StylesDirectory, scriptFilename) : $"Scripts/{scriptFilename}";

            HtmlNode scriptNode = HtmlNode.CreateNode("<script></script>");
            scriptNode.Attributes.Add("src", scriptFilePath);

            return scriptNode;
        }
    }
}
