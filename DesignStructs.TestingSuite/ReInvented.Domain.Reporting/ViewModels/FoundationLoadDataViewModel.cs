using System;
using System.IO;
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
            ProjectInfo = new ProjectInfo();
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
            FoundationLoadData foundationLoadData = service.GenerateReportContent(ProjectInfo);

            DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(ProjectInfo.ProjectDirectory, ProjectInfo.Code));

            FoundationLoadDataService.CreateReportHtmlFile(projectDirectory);
            FoundationLoadDataService.CopyCssStyleFiles(projectDirectory);
            FoundationLoadDataService.CopyJavaScriptFiles(projectDirectory);
            FoundationLoadDataService.CreateReportContentsFile(projectDirectory, foundationLoadData);

        }


        #region Event Handlers

        private void OnSelectProjectDirectory()
        {
            ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog(ProjectInfo.ProjectDirectory);
        }

        #endregion

    }
}
