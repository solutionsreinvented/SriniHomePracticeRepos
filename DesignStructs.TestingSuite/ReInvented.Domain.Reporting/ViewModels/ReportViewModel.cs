using System;
using System.Windows.Input;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public abstract class ReportViewModel : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public ReportViewModel()
        {
            ProjectInfo = new ProjectInfo() { ProjectDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\Reports" };
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
            SelectProjectDirectoryCommand = new RelayCommand(OnSelectProjectDirectory, true);
            SelectRevisionHistoryFileCommand = new RelayCommand(OnSelectRevisionHistoryFile, true);
        }

        #endregion

        #region Public Properties

        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        protected StaadModelWrapper Wrapper { get; private set; }

        #endregion

        #region Commands

        public ICommand SelectProjectDirectoryCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand SelectRevisionHistoryFileCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Event Handlers

        private void OnSelectProjectDirectory()
        {
            ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog(ProjectInfo.ProjectDirectory);
        }

        protected abstract void OnSelectRevisionHistoryFile();

        protected virtual void OnGenerateReport()
        {
            if (ProjectInfo.ProjectDirectory == null || ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(ProjectInfo.ProjectDirectory)} or {nameof(ProjectInfo.Code)} or both null.");
            }
        }

        #endregion
    }
}
