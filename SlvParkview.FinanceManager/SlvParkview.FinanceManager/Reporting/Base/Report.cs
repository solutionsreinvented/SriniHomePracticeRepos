﻿using Newtonsoft.Json;

using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SlvParkview.FinanceManager.Reporting.Models.Base
{
    public abstract class Report : PropertyStore, IReport
    {
        #region Private Fields

        private protected string _reportTargetDirectory;

        #endregion

        #region Default Constructor

        public Report()
        {

        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string GeneratedOn => DateTime.Today.ToString("dd MMM yyyy");

        [JsonProperty]
        public string ApartmentName => "SLV Parkview Apartment";

        [JsonProperty]
        public string DocumentTitle => GetFileName();

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the contents of for the selected type of reports.
        /// </summary>
        public abstract void GenerateContents();

        /// <summary>
        /// Creates the required directories for generating various reports and then finally generates the report itself in HTML format.
        /// Creates a Json file also to store the data of the report.
        /// </summary>
        public virtual void SaveReport()
        {
            try
            {
                CreateRequiredDirectories();
                CreateJavaScriptFile();
                CreateHtmlFile();
                CreatePdfFromHtml();

                _ = MessageBox.Show("Report generated successfully!!!", "Generate report", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Generate report", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Private Helper Functions

        /// <summary>
        /// Serializes the contents of 'this' report object and provides the same for persistence.
        /// </summary>
        /// <returns>A serialized string</returns>
        private protected abstract string GetSerializedData();

        #endregion

        #region Private Abstract Methods

        /// <summary>
        /// Creates the HTML file using the templates.
        /// </summary>
        private protected abstract void CreateHtmlFile();

        /// <summary>
        /// Creates the JavaScript file  based on the template and embeds the Json content generated by this report.
        /// </summary>
        private protected abstract void CreateJavaScriptFile();

        /// <summary>
        /// Generates the filename for Html and JavaScript files without extension.
        /// </summary>
        private protected abstract string GetFileName();

        #endregion

        #region Private Virtual Methods

        /// <summary>
        /// Creates the main Reports directory and other required directories dependin on the type of report selected
        /// if they do not exist.
        /// </summary>
        private protected virtual void CreateRequiredDirectories()
        {
            /// Create if the Reports directory does not exists.
            if (!Directory.Exists(FileServiceProvider.ReportsDirectory))
            {
                _ = Directory.CreateDirectory(FileServiceProvider.ReportsDirectory);
            }
        }

        /// <summary>
        /// Concatenates the contents of the Json generated from this report and the JavaScript template.
        /// </summary>
        /// <param name="javascriptTemplateContent">Content of the template JavaScipt file.</param>
        /// <returns>The final JavaScript file content in a string form which can directly persisted.</returns>
        private protected virtual string ConcatenateJsonContentIn(string[] javascriptTemplateContent)
        {
            int jsonRegionStart = javascriptTemplateContent.ToList().FindIndex(l => l == $"// #region Content");
            int jsonRegionEnd = javascriptTemplateContent.ToList().FindIndex(l => l == "// #endregion");

            IEnumerable<string> contentBeforeJson = javascriptTemplateContent.Take(jsonRegionStart + 1);

            IEnumerable<string> contentAfterJson = javascriptTemplateContent.Skip(jsonRegionEnd);

            string jsonContent = $"{Environment.NewLine}var jsonContents = {Environment.NewLine}{GetSerializedData()};";

            string concatenatedContentBeforeJson = $"{string.Join(Environment.NewLine, contentBeforeJson)}";
            string concatenatedContentAfterJson = $"{Environment.NewLine}{string.Join(Environment.NewLine, contentAfterJson)}";

            string finalJsContent = $"{concatenatedContentBeforeJson}{jsonContent}{concatenatedContentAfterJson}";

            return finalJsContent;
        }

        /// <summary>
        /// Replaces the Script tag in the HTML template file to include the local JavaScript file so that the Json content
        /// gets rendered properly.
        /// </summary>
        /// <param name="htmlTemplateContent">Content of the Html template file.</param>
        /// <param name="htmlFileName">Name of the final Html file.</param>
        /// <returns>The final content of the Html file in the form of a <see cref="List{string}"/>.</returns>
        private protected virtual List<string> ConcatenateScriptTagIn(string[] htmlTemplateContent, string htmlFileName)
        {
            List<string> finalHtmlFileContent = new List<string>();

            int htmlScriptStart = htmlTemplateContent.ToList().FindIndex(l => l.Contains($"script"));

            IEnumerable<string> contentBeforeScript = htmlTemplateContent.Take(htmlScriptStart);

            IEnumerable<string> contentAfterJson = htmlTemplateContent.Skip(htmlScriptStart + 1);

            finalHtmlFileContent.AddRange(contentBeforeScript);
            finalHtmlFileContent.Add($"<script src=\"{htmlFileName.Replace(".html", ".js")}\"></script>");
            finalHtmlFileContent.AddRange(contentAfterJson);

            return finalHtmlFileContent;
        }

        #endregion

        private protected virtual void CreatePdfFromHtml()
        {
            string htmlFilePath = Path.Combine(_reportTargetDirectory, $"{GetFileName()}.html");
            string pdfFilePath = Path.Combine(_reportTargetDirectory, $"{GetFileName()}.pdf");


        }

    }
}
