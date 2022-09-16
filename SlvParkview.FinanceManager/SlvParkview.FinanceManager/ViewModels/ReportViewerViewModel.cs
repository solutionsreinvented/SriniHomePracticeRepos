using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewerViewModel : BaseViewModel
    {
        private SummaryViewModel _sender;
        private NavigationService _navigationService;

        public ReportViewerViewModel(SummaryViewModel sender, NavigationService navigationService, string reportFileFullPath)
        {
            _sender = sender;
            _navigationService = navigationService;
            ReportFileFullPath = reportFileFullPath;
        }

        public string ReportFileFullPath { get => Get<string>(); set => Set(value); }
    }
}
