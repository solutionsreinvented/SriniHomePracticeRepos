using System;
using System.Windows;
using System.Windows.Input;

using Newtonsoft.Json;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Base
{
    public class BaseViewModel : ErrorsEnabledPropertyStore
    {

    }

    public abstract class ReportViewModel : BaseViewModel
    {
        #region Default Constructor

        public ReportViewModel(StaadModelWrapper wrapper)
        {
            Wrapper = wrapper;

            Initialize();
        }

        #endregion

        #region Public Properties

        [JsonIgnore]
        public StaadModelWrapper Wrapper { get; protected set; }

        public Report Report { get => Get<Report>(); protected set => Set(value); }

        public string Title { get => Get<string>(); protected set => Set(value); }

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
                IReportDocumentsGenerationService documentsService = GetReportDocumentsGenerationService();
                documentsService.Generate();

                _ = MessageBox.Show("Report is succesfully generated!", "Report Documents Generation", MessageBoxButton.OK);
            }
            else
            {
                _ = MessageBox.Show("It appears that the report content is not generated. Make sure to generate the report content before generating the documents!", "Report Documents Generation", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Virtual Methods

        protected virtual void GenerateReportContent()
        {
            Report.DataSource.Engineer = Report.ProjectInfo.ScrutinyHistory.Originator.FullName;
            Report.DataSource.PreparedOn = DateTime.Now.ToString("F");
            Report.DataSource.ProjectCode = Report.ProjectInfo.Code;
            Report.DataSource.ProjectName = Report.ProjectInfo.Name;
            Report.DataSource.StaadFilename = Wrapper.StaadInstance.GetStaadFileNameOnly();
        }

        #endregion

        #region Abstract Methods

        protected abstract IReportDocumentsGenerationService GetReportDocumentsGenerationService();

        #endregion

        #region Private Helpers

        protected virtual void Initialize()
        {
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
        }

        #endregion
    }
}
