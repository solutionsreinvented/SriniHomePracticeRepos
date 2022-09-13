using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using System.Collections.Generic;

namespace ApartmentFinanceManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService; 

        #endregion

        public ReportViewModel(SummaryViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
        {
            _sender = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        public List<FlatTransactionRecord> TransactionsSummary => FlatToBeProcessed.TransactionsSummary;

    }
}
