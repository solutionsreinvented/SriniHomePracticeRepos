using System.ComponentModel;
using System.Linq;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class FLDReportViewModel : ReportViewModel, INotifyPropertyChanged
    {
        #region Default Constructor

        public FLDReportViewModel(StaadModelWrapper wrapper) : base(wrapper)
        {
            Title = "Report - Foundation Load Data";
        }

        #endregion

        #region Abstract Methods Implementation

        protected override void GenerateReportContent()
        {
            base.GenerateReportContent();
            Report.Content = FoundationLoadDataService.Generate(Report.ProjectData, Enumerable.Range(601, 15));
        }

        protected override IReportDocumentsGenerationService GetReportDocumentsGenerationService()
        {
            return new FLDReportDocumentsGenerationService(Report, true);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Report = new FLDReport();
        }

        #endregion
    }
}
