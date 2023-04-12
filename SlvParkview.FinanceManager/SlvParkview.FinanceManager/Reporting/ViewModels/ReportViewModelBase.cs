using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.ViewModels;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    public class ReportViewModelBase : PropertyStore
    {
        #region Private Fields

        private protected readonly SummaryViewModel _summaryViewModel;
        private protected readonly NavigationService _navigationService;

        #endregion

        #region Parameterized Constructor

        public ReportViewModelBase(SummaryViewModel summaryViewModel, NavigationService navigationService)
        {
            _summaryViewModel = summaryViewModel;
            _navigationService = navigationService;

            Initialize();
        }

        #endregion

        #region Public Properties

        public Block Block { get => Get<Block>(); set => Set(value); }

        public IReport Report { get => Get<IReport>(); set => Set(value); }

        #endregion

        #region Private Helpers

        private protected virtual void Initialize()
        {
            Block = _summaryViewModel.Block;
        }

        #endregion

    }
}
