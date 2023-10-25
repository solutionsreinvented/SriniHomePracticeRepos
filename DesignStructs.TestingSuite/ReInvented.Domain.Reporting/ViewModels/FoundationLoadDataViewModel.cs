using System;
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
    public class FoundationLoadDataViewModel : ErrorsEnabledPropertyStore
    {
        public FoundationLoadDataViewModel()
        {
            ProjectInfo = new ProjectInfo() { ProjectDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\Reports" };
            GenerateFoundationLoadDataCommand = new RelayCommand(OnGenerateFoundationLoadData, true);
            SelectProjectDirectoryCommand = new RelayCommand(OnSelectProjectDirectory, true);
        }

        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        public ICommand GenerateFoundationLoadDataCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand SelectProjectDirectoryCommand { get; set; }


        private void OnGenerateFoundationLoadData()
        {
            if (ProjectInfo.ProjectDirectory == null || ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(ProjectInfo.ProjectDirectory)} or {nameof(ProjectInfo.Code)} or both null.");
            }

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
                MessageBox.Show("It appears that the analysis results are not available. Make sure to run the analysis before generating foundation load data!", "Foundation Load Data", MessageBoxButton.OK);
            }
        }


        #region Event Handlers

        private void OnSelectProjectDirectory()
        {
            ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog(ProjectInfo.ProjectDirectory);
        }

        #endregion

    }
}
