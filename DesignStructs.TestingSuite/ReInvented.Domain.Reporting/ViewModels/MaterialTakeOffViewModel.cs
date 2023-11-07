using System.ComponentModel;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class MaterialTakeOffViewModel : ReportViewModel<MaterialTakeOff>, INotifyPropertyChanged
    {
        #region Default Constructor

        public MaterialTakeOffViewModel()
        {

        }

        #endregion

        #region Abstract Methods Implementation

        protected override void GenerateReportContent()
        {
            Report.Content = MaterialTakeOffService.Generate(Wrapper);
        }

        protected override IReportDocumentsGenerationService<MaterialTakeOff> GetReportDocumentsGenerationService()
        {
            return new MTOReportDocumentsGenerationService(Report, true);
        }

        #endregion
    }
}
