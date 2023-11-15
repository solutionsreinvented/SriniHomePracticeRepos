using System;
using System.IO;

using HtmlAgilityPack;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Services;

namespace ReInvented.Domain.Reporting.Base
{
    public abstract class ReportDocumentsGenerationService : IReportDocumentsGenerationService
    {
        #region Parameterized Constructor

        public ReportDocumentsGenerationService(Report report, bool useAbsolutePaths)
        {
            Report = report;
            UseAbsolutePaths = useAbsolutePaths;

            SetFileNames();
        }

        #endregion

        #region Main Methods

        public virtual void Generate()
        {
            ProjectReportsDirectory = Directory.CreateDirectory(Path.Combine(Report.ProjectInfo.ProjectDirectory, "Reports"));

            CreateReportHtmlFile();

            if (!UseAbsolutePaths)
            {
                CopyCssStyleFiles();
                CopyJavaScriptFiles();
            }

            CreateReportContentsFile();
            CreateDocumentFile();
        }

        private void CreateDocumentFile()
        {
            if (string.IsNullOrWhiteSpace(Report.Document.RevisionHistoryFilePath))
            {
                Report.Document.RevisionHistoryFilePath = Path.Combine(Report.ProjectInfo.ProjectDirectory, $"{Report.Document.Number}.{FileExtensions.RevisionHistory}");
            }

            JsonDataSerializer<Document> serializer = new JsonDataSerializer<Document>();
            string serialized = "const DocumentData = " + serializer.Serialize(Report.Document);
            File.WriteAllText(Report.Document.RevisionHistoryFilePath, serialized);
        }

        #endregion

        #region Protected Properties

        protected Report Report { get; set; }

        protected DirectoryInfo ProjectReportsDirectory { get; set; }

        protected bool UseAbsolutePaths { get; set; }

        protected string ReportSpecificContentsFileName { get; set; }

        protected string ReportSpecificHtmlFileName { get; set; }

        protected string ReportSpecificCssFileName { get; set; }

        protected string ReportSpecificJavaScriptFileName { get; set; }

        #endregion

        #region Abstract Methods

        protected abstract void SetFileNames();

        protected abstract void CopyJavaScriptFiles();

        protected abstract HtmlDocument AppendScriptTagsToBodyElement(HtmlDocument htmlDocument, bool useAbsolutePaths);

        #endregion

        #region Virtual Methods

        protected virtual void CreateReportHtmlFile()
        {
            string htmlSourceFileFullPath = Path.Combine(FileServiceProvider.TemplatesDirectory, "Pages", ReportSpecificHtmlFileName);
            string htmlDestinationFileFullPath = Path.Combine(ProjectReportsDirectory.FullName, ReportSpecificHtmlFileName);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlSourceFileFullPath);

            htmlDocument = LinkCssAndScriptsTo(htmlDocument, UseAbsolutePaths);

            htmlDocument.Save(htmlDestinationFileFullPath);
        }

        protected virtual void CopyCssStyleFiles()
        {
            string sourceStylesDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Styles");
            string destinationStylesDirectory = Path.Combine(ProjectReportsDirectory.FullName, "Styles");

            if (!Directory.Exists(destinationStylesDirectory))
            {
                _ = Directory.CreateDirectory(destinationStylesDirectory);
            }

            File.Copy(Path.Combine(sourceStylesDirectory, ReportFileNames.CssCommon), Path.Combine(destinationStylesDirectory, ReportFileNames.CssCommon), true);
            File.Copy(Path.Combine(sourceStylesDirectory, ReportSpecificCssFileName), Path.Combine(destinationStylesDirectory, ReportSpecificCssFileName), true);
        }

        protected virtual void CreateReportContentsFile()
        {
            string dataFilename = Report.Content.GetType() == typeof(FoundationLoadData) ? ReportFileNames.ContentsFoundationLoadData : ReportFileNames.ContentsMTO;

            DirectoryInfo dataDirectory = Directory.CreateDirectory(Path.Combine(ProjectReportsDirectory.FullName, "Data"));

            JsonDataSerializer<Report> serializer = new JsonDataSerializer<Report>();
            string seializedContent = "const ReportContent = " + serializer.Serialize(Report, JsonSerializerSettingsProvider.MinifiedSettings());

            File.WriteAllText(Path.Combine(dataDirectory.FullName, dataFilename), seializedContent);
        }

        #endregion

        #region Private Helpers

        private HtmlDocument LinkCssAndScriptsTo(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException($"{nameof(htmlDocument)} cannot be null or empty.");
            }

            htmlDocument = htmlDocument.RemoveAllExistingCssLinkTagsFromHeadElement()
                                       .RemoveAllExistingScriptTagsFromBodyElement();

            htmlDocument = AppendCssLinkTagsToHeadElement(htmlDocument, useAbsolutePaths);
            htmlDocument = AppendScriptTagsToBodyElement(htmlDocument, useAbsolutePaths);

            return htmlDocument;
        }

        private HtmlDocument AppendCssLinkTagsToHeadElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode head = htmlDocument.GetHeadElementNode();

            if (head != null)
            {
                _ = head.AppendChild(HtmlNodeServices.CreateStylesheetNodeWithAttributes(ReportFileNames.CssCommon, useAbsolutePaths));
                _ = head.AppendChild(HtmlNodeServices.CreateStylesheetNodeWithAttributes(ReportSpecificCssFileName, useAbsolutePaths));
            }

            return htmlDocument;
        }

        #endregion
    }
}
