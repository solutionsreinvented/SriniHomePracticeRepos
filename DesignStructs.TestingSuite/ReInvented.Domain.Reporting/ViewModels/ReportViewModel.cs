using System;
using System.Windows.Input;

using ReInvented.DataAccess.Services;
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

        protected StaadModelWrapper Wrapper { get; private set; }

        #endregion

        #region Commands

        public ICommand SelectProjectDirectoryCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand SelectRevisionHistoryFileCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Event Handlers

        protected abstract void OnSelectProjectDirectory();

        protected abstract void OnSelectRevisionHistoryFile();

        protected abstract void OnGenerateReport();

        #endregion
    }
}
