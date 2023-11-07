using System.Windows.Input;

using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.ViewModels
{

    public class HomeViewModel : ErrorsEnabledPropertyStore
    {
        public HomeViewModel()
        {
            ReportViewModel = new FoundationLoadDataViewModel();
            MoveToNextReportCommand = new RelayCommand(OnMoveToNextReportCommand, true);
        }

        public BaseViewModel ReportViewModel { get => Get<BaseViewModel>(); set => Set(value); }

        public ICommand MoveToNextReportCommand { get; set; }

        private void OnMoveToNextReportCommand()
        {
            if (ReportViewModel.GetType() == typeof(FoundationLoadDataViewModel))
            {
                ReportViewModel = new MaterialTakeOffViewModel();
            }
            else
            {
                ReportViewModel = new FoundationLoadDataViewModel();
            }
        }

    }
}
