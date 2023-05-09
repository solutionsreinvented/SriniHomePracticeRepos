using System.Windows.Input;

using ReInvented.Shared.Commands;

using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewerViewModel : BaseViewModel
    {
        private readonly ReportingViewModel _sender;
        private readonly NavigationService _navigationService;

        public ReportViewerViewModel(ReportingViewModel sender, NavigationService navigationService)
        {
            _sender = sender;
            _navigationService = navigationService;

            Report = sender.CurrentReportViewModel.Report;

            Initialize();
        }

        private void Initialize()
        {
            GoBackCommand = new RelayCommand(OnGoBack, true);
        }

        private void OnGoBack()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        public ICommand GoBackCommand { get => Get<ICommand>(); set => Set(value); }

        public IReport Report { get => Get<IReport>(); private set { Set(value); RaisePropertyChanged(nameof(HtmlFileFullPath)); } }

        public string HtmlFileFullPath => Report.HtmlFilePath;

    }
}
