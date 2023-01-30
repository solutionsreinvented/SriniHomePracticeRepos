using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewerViewModel : BaseViewModel
    {
        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        public ReportViewerViewModel(SummaryViewModel sender, NavigationService navigationService, string reportFileFullPath)
        {
            _sender = sender;
            _navigationService = navigationService;
            ReportFileFullPath = reportFileFullPath;
        }

        public string ReportFileFullPath { get => Get<string>(); set => Set(value); }
    }
}
