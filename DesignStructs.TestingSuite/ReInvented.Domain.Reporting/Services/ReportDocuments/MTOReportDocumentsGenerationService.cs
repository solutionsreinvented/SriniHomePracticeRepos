using System.IO;

using HtmlAgilityPack;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Services;

namespace ReInvented.Domain.Reporting.Services
{
    public class MTOReportDocumentsGenerationService : ReportDocumentsGenerationService<MaterialTakeOff>, IReportDocumentsGenerationService<MaterialTakeOff>
    {
        #region Parameterized Constructor

        public MTOReportDocumentsGenerationService(Report<MaterialTakeOff> report, bool useAbsolutePaths) : base(report, useAbsolutePaths)
        {

        }

        #endregion

        #region Abstract Methods Implementation

        protected override void SetFileNames()
        {
            ReportSpecificHtmlFileName = ReportFileNames.HtmlMTO;
            ReportSpecificCssFileName = ReportFileNames.CssMTO;
            ReportSpecificContentsFileName = ReportFileNames.ContentsMTO;
            ReportSpecificJavaScriptFileName = ReportFileNames.JavaScriptMTO;
        }

        protected override void CopyJavaScriptFiles()
        {
            string sourceScriptsDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Scripts");
            string destinationScriptsDirectory = Path.Combine(ProjectReportsDirectory.FullName, "Scripts");

            if (!Directory.Exists(destinationScriptsDirectory))
            {
                _ = Directory.CreateDirectory(destinationScriptsDirectory);
            }

            File.Copy(Path.Combine(sourceScriptsDirectory, ReportSpecificJavaScriptFileName), Path.Combine(destinationScriptsDirectory, ReportSpecificJavaScriptFileName), true);
        }

        protected override HtmlDocument AppendScriptTagsToBodyElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode body = htmlDocument.GetBodyElementNode();

            if (body != null)
            {
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes($"Data/{ReportSpecificContentsFileName}"));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportSpecificJavaScriptFileName, useAbsolutePaths));
            }

            return htmlDocument;
        }


        #endregion
    }
}
