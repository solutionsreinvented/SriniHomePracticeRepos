using System.ComponentModel;
using System.Linq;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class FoundationLoadDataViewModel : ReportViewModel<FoundationLoadData>, INotifyPropertyChanged
    {
        #region Default Constructor

        public FoundationLoadDataViewModel(StaadModelWrapper wrapper) : base(wrapper)
        {
            Title = "Report - Foundatio Load Data";
        }

        #endregion

        #region Abstract Methods Implementation

        protected override void GenerateReportContent()
        {
            base.GenerateReportContent();
            Report.Content = FoundationLoadDataService.Generate(Report.ProjectInfo, Enumerable.Range(601, 15));
        }

        protected override IReportDocumentsGenerationService GetReportDocumentsGenerationService()
        {
            return new FDLReportDocumentsGenerationService(Report, true);
        }

        #endregion
    }
}
