using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewerViewModel : BaseViewModel
    {
        private readonly SummaryViewModel _summaryViewModel;
        private readonly NavigationService _navigationService;

        public ReportViewerViewModel(SummaryViewModel sender, NavigationService navigationService, IReport report)
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            Report = report;
        }

        public IReport Report { get => Get<IReport>(); private set { Set(value); RaisePropertyChanged(nameof(HtmlFileFullPath)); } }

        public string HtmlFileFullPath => Report.HtmlFilePath;

    }
}
