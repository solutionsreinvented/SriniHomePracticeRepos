using System.Windows.Input;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class HomeViewModel : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public HomeViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public StaadModelWrapper Wrapper { get; private set; }

        public BaseViewModel ReportViewModel { get => Get<BaseViewModel>(); set => Set(value); }

        public ICommand CreateFLDReportCommand { get; private set; }

        public ICommand CreateMTOReportCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnCreateFLDReportCommand()
        {
            if (ReportViewModel.GetType() != typeof(FLDReportViewModel))
            {
                ReportViewModel = new FLDReportViewModel(Wrapper);
            }
        }
        private void OnCreateMTOReportCommand()
        {
            if (ReportViewModel.GetType() != typeof(MTOReportViewModel))
            {
                ReportViewModel = new MTOReportViewModel(Wrapper);
            }
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            StaadModel model = new StaadModel();
            Wrapper = model.StaadWrapper;

            ReportViewModel = new MTOReportViewModel(Wrapper);

            CreateFLDReportCommand = new RelayCommand(OnCreateFLDReportCommand, true);
            CreateMTOReportCommand = new RelayCommand(OnCreateMTOReportCommand, true);
        }

        #endregion
    }
}
