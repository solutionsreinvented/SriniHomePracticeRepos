using System.IO;

using HtmlAgilityPack;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Extensions;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public class FDLReportDocumentsGenerationService : ReportDocumentsGenerationService<FoundationLoadData>, IReportDocumentsGenerationService<FoundationLoadData>
    {
        #region Parameterized Constructor

        public FDLReportDocumentsGenerationService(Report<FoundationLoadData> report, bool useAbsolutePaths) : base(report, useAbsolutePaths)
        {

        }

        #endregion

        #region Abstract Methods Implementation

        protected override void CreateReportHtmlFile()
        {
            string htmlSourceFileFullPath = Path.Combine(FileServiceProvider.TemplatesDirectory, "Pages", ReportFileNames.HtmlFoundationLoadData);
            string htmlDestinationFileFullPath = Path.Combine(ProjectReportsDirectory.FullName, ReportFileNames.HtmlFoundationLoadData);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlSourceFileFullPath);
            htmlDocument = htmlDocument.LinkCssAndScriptsTo(UseAbsolutePaths);

            htmlDocument.Save(htmlDestinationFileFullPath);
        }

        protected override void CopyCssStyleFiles()
        {
            string sourceStylesDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Styles");
            string destinationStylesDirectory = Path.Combine(ProjectReportsDirectory.FullName, "Styles");

            if (!Directory.Exists(destinationStylesDirectory))
            {
                _ = Directory.CreateDirectory(destinationStylesDirectory);
            }

            File.Copy(Path.Combine(sourceStylesDirectory, ReportFileNames.CssCommon), Path.Combine(destinationStylesDirectory, ReportFileNames.CssCommon), true);
            File.Copy(Path.Combine(sourceStylesDirectory, ReportFileNames.CssFoundationLoadData), Path.Combine(destinationStylesDirectory, ReportFileNames.CssFoundationLoadData), true);
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
            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptFoundationLoadData), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptFoundationLoadData), true);
        }

        #endregion
    }
}
