using System.Windows.Input;

using ReInvented.Shared.Commands;

using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewerViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly ReportingViewModel _sender;
        private readonly NavigationService _navigationService;
        #endregion

        #region Parameterized Constructor
        public ReportViewerViewModel(ReportingViewModel sender, NavigationService navigationService)
        {
            _sender = sender;
            _navigationService = navigationService;

            Report = sender.CurrentReportViewModel.Report;
            HtmlFileFullPath = Report.HtmlFilePath;
            //HtmlFileFullPath = @"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\Reports\Templates\Pages\api650.html";

            Initialize();
        }
        #endregion

        #region Properties
        public IReport Report { get => Get<IReport>(); private set { Set(value); RaisePropertyChanged(nameof(HtmlFileFullPath)); } }

        public string HtmlFileFullPath { get => Get<string>(); private set => Set(value); }
        #endregion

        #region Commands
        public ICommand GoBackCommand { get => Get<ICommand>(); set => Set(value); }
        #endregion

        #region Command Handlers
        private void OnGoBack()
        {
            _navigationService.CurrentViewModel = _sender;
        }
        #endregion

        #region Private Helpers
        private void Initialize()
        {
            GoBackCommand = new RelayCommand(OnGoBack, true);
        }
        #endregion
    }
}
