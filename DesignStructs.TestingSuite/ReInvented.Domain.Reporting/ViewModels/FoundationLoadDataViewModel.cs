using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class FoundationLoadDataViewModel : ReportViewModel, INotifyPropertyChanged
    {
        #region Default Constructor

        public FoundationLoadDataViewModel()
        {
            
        }

        #endregion

        #region Public Properties

        public Report<FoundationLoadData> Report { get; private set; }

        #endregion

        #region Event Handlers

        protected override void OnSelectProjectDirectory()
        {
            Report.ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog(Report.ProjectInfo.ProjectDirectory);
        }

        protected override void OnGenerateReport()
        {
            if (Report.ProjectInfo.ProjectDirectory == null || Report.ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(Report.ProjectInfo.ProjectDirectory)} or {nameof(Report.ProjectInfo.Code)} or both null.");
            }

            Report.Content = FoundationLoadDataService.Generate(Report.ProjectInfo, Enumerable.Range(601, 15));

            if (Report.Content != null)
            {
                bool useAbsolutePaths = true;

                DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(ProjectInfo.ProjectDirectory, ProjectInfo.Code));

                FoundationLoadDataService.CreateReportHtmlFile(projectDirectory, useAbsolutePaths);

                if (!useAbsolutePaths)
                {
                    FoundationLoadDataService.CopyCssStyleFiles(projectDirectory);
                    FoundationLoadDataService.CopyJavaScriptFiles(projectDirectory);
                }

                FoundationLoadDataService.CreateReportContentsFile(projectDirectory, Report.Content);
            }
            else
            {
                _ = MessageBox.Show("It appears that the analysis results are not available. Make sure to run the analysis before generating foundation load data!", "Foundation Load Data", MessageBoxButton.OK);
            }
        }

        protected override void OnSelectRevisionHistoryFile()
        {
            FileFilter filter = new FileFilter("Revision History Files", FileExtensions.RevisionHistoryFile);

            Report.DocumentInfo.RevisionHistoryFilePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(filter);
        }

        #endregion
    }
}
