using SlvParkview.FinanceManager.Services;
using System;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ModifyFlatViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;

        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ModifyFlatViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor
        public ModifyFlatViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService)
            : this()
        {
            _summaryViewModel = summaryViewModel;
            _navigationService = navigationService;
        }
        #endregion

        #region Private Helpers

        private void Initialize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
