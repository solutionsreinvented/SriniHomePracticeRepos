using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

namespace ApartmentFinanceManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private NavigationService _navigationService;

        public ReportViewModel(NavigationService navigationService, Flat flatToBeProcessed)
        {
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }
    }
}
