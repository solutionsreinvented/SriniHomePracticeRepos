using System.IO;

using HtmlAgilityPack;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Services;

namespace ReInvented.Domain.Reporting.Services
{
    public class FLDReportDocumentsGenerationService : ReportDocumentsGenerationService, IReportDocumentsGenerationService
    {
        #region Parameterized Constructor

        public FLDReportDocumentsGenerationService(Report report, bool useAbsolutePaths) : base(report, useAbsolutePaths)
        {

        }

        #endregion

        #region Abstract Methods Implementation

        protected override void SetFileNames()
        {
            ReportSpecificHtmlFileName = ReportFileNames.HtmlFoundationLoadData;
            ReportSpecificCssFileName = ReportFileNames.CssFoundationLoadData;
            ReportSpecificContentsFileName = ReportFileNames.ContentsFoundationLoadData;
            ReportSpecificJavaScriptFileName = ReportFileNames.JavaScriptFoundationLoadData;
        }

        protected override void CopyJavaScriptFiles()
        {
            string sourceScriptsDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Scripts");
            string destinationScriptsDirectory = Path.Combine(ProjectReportsDirectory.FullName, "Scripts");

            if (!Directory.Exists(destinationScriptsDirectory))
            {
                _ = Directory.CreateDirectory(destinationScriptsDirectory);
            }

            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptCanvasGraphics), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptCanvasGraphics), true);
            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptSupportLayoutHelpers), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptSupportLayoutHelpers), true);
            File.Copy(Path.Combine(sourceScriptsDirectory, ReportSpecificJavaScriptFileName), Path.Combine(destinationScriptsDirectory, ReportSpecificJavaScriptFileName), true);
        }

        protected override HtmlDocument AppendScriptTagsToBodyElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode body = htmlDocument.GetBodyElementNode();

            if (body != null)
            {
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes($"Data/{ReportSpecificContentsFileName}"));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportFileNames.JavaScriptCanvasGraphics, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportFileNames.JavaScriptSupportLayoutHelpers, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportFileNames.JavaScriptShared, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportSpecificJavaScriptFileName, useAbsolutePaths));
            }

            return htmlDocument;
        }

        #endregion
    }
}
