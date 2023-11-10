using System.Windows.Input;

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

        public ICommand CreateFDLReportCommand { get; set; }

        public ICommand CreateMTOReportCommand { get; set; }

        #endregion

        #region Command Handlers

        private void OnCreateFDLReportCommand()
        {
            if (ReportViewModel.GetType() != typeof(FoundationLoadDataViewModel))
            {
                ReportViewModel = new FoundationLoadDataViewModel(Wrapper);
            }
        }
        private void OnCreateMTOReportCommand()
        {
            if (ReportViewModel.GetType() != typeof(MaterialTakeOffViewModel))
            {
                ReportViewModel = new MaterialTakeOffViewModel(Wrapper);
            }
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            StaadModel model = new StaadModel();
            Wrapper = model.StaadWrapper;

            ReportViewModel = new MaterialTakeOffViewModel(Wrapper);

            CreateFDLReportCommand = new RelayCommand(OnCreateFDLReportCommand, true);
            CreateMTOReportCommand = new RelayCommand(OnCreateMTOReportCommand, true);
        }

        #endregion
    }
}
