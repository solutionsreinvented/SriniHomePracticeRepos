using System.ComponentModel;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class MTOReportViewModel : ReportViewModel, INotifyPropertyChanged
    {
        #region Default Constructor

        public MTOReportViewModel(StaadModelWrapper wrapper) : base(wrapper)
        {
            Title = "Report - Material Take Off";
        }

        #endregion

        #region Abstract Methods Implementation

        protected override void GenerateReportContent()
        {
            base.GenerateReportContent();
            Report.Content = MaterialTakeOffService.Generate(Wrapper, (Report as MTOReport).Contingencies);
        }

        protected override IReportDocumentsGenerationService GetReportDocumentsGenerationService()
        {
            return new MTOReportDocumentsGenerationService(Report, true);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Report = new MTOReport();
        }

        #endregion
    }
}
