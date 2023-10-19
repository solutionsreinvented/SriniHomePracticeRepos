using System.Collections.Generic;

using HtmlAgilityPack;

namespace ReInvented.Domain.Reporting.Services
{
    public class HtmlDocumentManager
    {
        public static List<HtmlNode> GetScriptElementsInHead(HtmlDocument htmlDoc)
        {
            List<HtmlNode> scriptElementsInHead = new List<HtmlNode>();

            foreach (HtmlNode scriptElement in htmlDoc.DocumentNode.Descendants("script"))
            {
                if (scriptElement.ParentNode.Name == "head")
                {
                    scriptElementsInHead.Add(scriptElement);
                }
            }

            return scriptElementsInHead;
        }
        public static List<HtmlNode> GetScriptElementsInBody(HtmlDocument htmlDoc)
        {
            List<HtmlNode> scriptElementsInBody = new List<HtmlNode>();

            foreach (HtmlNode scriptElement in htmlDoc.DocumentNode.Descendants("script"))
            {
                if (scriptElement.ParentNode.Name == "body")
                {
                    scriptElementsInBody.Add(scriptElement);
                }
            }

            return scriptElementsInBody;
        }
    }
}
