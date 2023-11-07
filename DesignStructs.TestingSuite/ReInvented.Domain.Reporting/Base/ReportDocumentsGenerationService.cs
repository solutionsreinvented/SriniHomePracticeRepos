using System;
using System.IO;
using System.Windows;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.Base
{
    public abstract class ReportDocumentsGenerationService<T> : IReportDocumentsGenerationService<T>
    {
        #region Parameterized Constructor

        public ReportDocumentsGenerationService(Report<T> report, bool useAbsolutePaths)
        {
            Report = report;
            UseAbsolutePaths = useAbsolutePaths;
        }

        #endregion

        #region Main Methods

        public virtual void Generate()
        {
            if (Report.ProjectInfo.ProjectDirectory == null || Report.ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(Report.ProjectInfo.ProjectDirectory)} or {nameof(Report.ProjectInfo.Code)} or both null.");
            }

            if (Report.Content != null)
            {
                bool useAbsolutePaths = true;

                ProjectReportsDirectory = Directory.CreateDirectory(Path.Combine(Report.ProjectInfo.ProjectDirectory, "Reports"));

                CreateReportHtmlFile();

                if (!useAbsolutePaths)
                {
                    CopyCssStyleFiles();
                    CopyJavaScriptFiles();
                }

                CreateReportContentsFile();
            }
            else
            {
                _ = MessageBox.Show("It appears that the report content is not generated. Make sure to generate the report content before generating the documents!", "Report Documents Generation", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Private Properties

        protected Report<T> Report { get; set; }

        protected DirectoryInfo ProjectReportsDirectory { get; set; }

        protected bool UseAbsolutePaths { get; set; }

        #endregion

        #region Abstract Methods

        protected abstract void CreateReportHtmlFile();

        protected abstract void CopyCssStyleFiles();

        protected abstract void CopyJavaScriptFiles();

        #endregion

        #region Virtual Methods

        protected virtual void CreateReportContentsFile()
        {
            DirectoryInfo dataDirectory = Directory.CreateDirectory(Path.Combine(ProjectReportsDirectory.FullName, "Data"));

            JsonDataSerializer<Report<T>> serializer = new JsonDataSerializer<Report<T>>();
            string seializedContent = "const ReportContent = " + serializer.Serialize(Report, JsonSerializerSettingsProvider.MinifiedSettings());

            string dataFilename = typeof(T) == typeof(FoundationLoadData) ? ReportFileNames.ContentsFoundationLoadData : ReportFileNames.ContentsMTO;

            File.WriteAllText(Path.Combine(dataDirectory.FullName, dataFilename), seializedContent);
        }

        #endregion
    }
}
