using System;
using System.IO;
using System.Windows;

using HtmlAgilityPack;

using ReInvented.DataAccess;
using ReInvented.DataAccess.NameProviders;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Optimization.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;
using ReInvented.Shared.Web.Extensions;
using ReInvented.Shared.Web.Services;

namespace ReInvented.Domain.Optimization.Services
{
    public class POReportDocumentsGenerationService
    {
        #region Parameterized Constructor

        public POReportDocumentsGenerationService(PlatesOptimizationReport report, IDialogService dialogService)
        {
            if (report == null)
            {
                throw new ArgumentNullException($"{nameof(report)} shall not be null!");
            }
            if (report.GetType() != typeof(PlatesOptimizationReport))
            {
                throw new ArgumentException($"{nameof(report)} shall be of type {typeof(PlatesOptimizationReport)}");
            }

            Report = report;
            DialogService = dialogService ?? throw new ArgumentNullException($"{nameof(dialogService)} shall not be null!");
        }

        #endregion

        #region Readonly Properties

        public PlatesOptimizationReport Report { get; set; }

        public IDialogService DialogService { get; set; }

        #endregion

        #region Public Functions

        public void SaveReport()
        {
            string outputFileFullPath = Report.OutputFiles.ReportDataFileJson;
            string outputHtmlFullPath = Report.OutputFiles.ReportHtmlFile;

            JsonDataSerializer<PlatesOptimizationReport> serializer = new JsonDataSerializer<PlatesOptimizationReport>();
            string serialized = "const content = " + serializer.Serialize(Report, JsonSerializerSettingsProvider.Minified);

            string sourceHtml = Path.Combine(DirectoryPaths.ReportsPages, $"plates-optimization.{FileExtensions.Html}");

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.Load(sourceHtml);
            htmlDocument = LinkCssAndScriptsTo(htmlDocument, true);

            File.WriteAllText(outputFileFullPath, serialized);
            _ = CreateReportHtmlFile(htmlDocument, outputHtmlFullPath);
        }

        #endregion

        #region Private Helpers

        private HtmlDocument LinkCssAndScriptsTo(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException($"{nameof(htmlDocument)} cannot be null or empty.");
            }

            htmlDocument = htmlDocument.RemoveAllExistingCssLinkTagsFromHeadElement().RemoveAllExistingScriptTagsFromBodyElement();
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
                _ = head.AppendChild(HtmlNodeServices.CreateStylesheetNodeWithAttributes(ReportFileNames.CssPlatesOptimization, useAbsolutePaths));
            }

            return htmlDocument;
        }

        private HtmlDocument AppendScriptTagsToBodyElement(HtmlDocument htmlDocument, bool useAbsolutePaths)
        {
            HtmlNode body = htmlDocument.GetBodyElementNode();

            if (body != null)
            {
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes($"{Report.OutputFiles.ReportDataFileJson}"));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportFileNames.JavaScriptShared, useAbsolutePaths));
                _ = body.AppendChild(HtmlNodeServices.CreateScriptNodeWithAttributes(ReportFileNames.JavaScriptPlatesOptimization, useAbsolutePaths));
            }

            return htmlDocument;
        }

        private bool CreateReportHtmlFile(HtmlDocument htmlDocument, string htmlDestinationFileFullPath)
        {
            if (File.Exists(htmlDestinationFileFullPath))
            {
                MessageBoxResult result = MessageService.ShowMessage(DialogService, "The specified report file already exists! Do you want to override the file?", "Create reports", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    htmlDocument.Save(htmlDestinationFileFullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                htmlDocument.Save(htmlDestinationFileFullPath);
                return true;
            }
        }

        #endregion
    }
}
