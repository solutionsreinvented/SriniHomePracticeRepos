using System;
using System.Windows;
using System.Windows.Input;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class BaseViewModel : ErrorsEnabledPropertyStore
    {

    }

    public abstract class ReportViewModel<T> : BaseViewModel
    {
        #region Default Constructor

        public ReportViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        protected StaadModelWrapper Wrapper { get; private set; }

        public Report<T> Report { get; private set; }

        #endregion

        #region Commands

        public ICommand GenerateReportCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Event Handlers

        private void OnGenerateReport()
        {
            if (Report.ProjectInfo.ProjectDirectory == null || Report.ProjectInfo.Code == null)
            {
                throw new ArgumentNullException($"{nameof(Report.ProjectInfo.ProjectDirectory)} or {nameof(Report.ProjectInfo.Code)} or both null.");
            }

            GenerateReportContent();

            if (Report.Content != null)
            {
                IReportDocumentsGenerationService<T> documentsService = GetReportDocumentsGenerationService();
                documentsService.Generate();
            }
            else
            {
                _ = MessageBox.Show("It appears that the analysis results are not available. Make sure to run the analysis before generating foundation load data!", "Foundation Load Data", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Abstract Methods

        protected abstract void GenerateReportContent();

        protected abstract IReportDocumentsGenerationService<T> GetReportDocumentsGenerationService();

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);

            Report = new Report<T>();
        }

        #endregion
    }
}
