using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class ReportViewModel : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public ReportViewModel()
        {
            ProjectInfo = new ProjectInfo() { ProjectDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\Reports" };
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
            SelectProjectDirectoryCommand = new RelayCommand(OnSelectProjectDirectory, true);
        }

        #endregion

        #region Public Properties

        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        #endregion

        #region Commands

        public ICommand SelectProjectDirectoryCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Event Handlers

        private void OnSelectProjectDirectory()
        {
            ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog(ProjectInfo.ProjectDirectory);
        }

        protected virtual void OnGenerateReport()
        {
            if (ProjectInfo.ProjectDirectory == null || ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(ProjectInfo.ProjectDirectory)} or {nameof(ProjectInfo.Code)} or both null.");
            }
        }

        #endregion
    }

    public class FoundationLoadDataViewModel : ReportViewModel, INotifyPropertyChanged
    {
        public FoundationLoadDataViewModel()
        {

        }

        protected override void OnGenerateReport()
        {
            base.OnGenerateReport();

            FoundationLoadDataService service = new FoundationLoadDataService();
            FoundationLoadData foundationLoadData = service.GenerateReportContent(ProjectInfo, Enumerable.Range(601, 15));

            if (foundationLoadData != null)
            {
                bool useAbsolutePaths = true;

                DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(ProjectInfo.ProjectDirectory, ProjectInfo.Code));

                FoundationLoadDataService.CreateReportHtmlFile(projectDirectory, useAbsolutePaths);
                if (!useAbsolutePaths)
                {
                    FoundationLoadDataService.CopyCssStyleFiles(projectDirectory);
                    FoundationLoadDataService.CopyJavaScriptFiles(projectDirectory);
                }
                FoundationLoadDataService.CreateReportContentsFile(projectDirectory, foundationLoadData);
            }
            else
            {
                _ = MessageBox.Show("It appears that the analysis results are not available. Make sure to run the analysis before generating foundation load data!", "Foundation Load Data", MessageBoxButton.OK);
            }
        }

    }
}
