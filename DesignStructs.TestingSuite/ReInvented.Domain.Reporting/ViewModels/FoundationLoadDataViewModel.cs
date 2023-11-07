using System.ComponentModel;
using System.Linq;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class FoundationLoadDataViewModel : ReportViewModel<FoundationLoadData>, INotifyPropertyChanged
    {
        #region Default Constructor

        public FoundationLoadDataViewModel()
        {

        }

        #endregion

        #region Abstract Methods Implementation

        protected override void GenerateReportContent()
        {
            Report.Content = FoundationLoadDataService.Generate(Report.ProjectInfo, Enumerable.Range(601, 15));
        }

        protected override IReportDocumentsGenerationService<FoundationLoadData> GetReportDocumentsGenerationService()
        {
            return new FDLReportDocumentsGenerationService(Report, true);
        }

        #endregion
    }
}
