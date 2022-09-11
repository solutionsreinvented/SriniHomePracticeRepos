using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

namespace ApartmentFinanceManager.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        public SummaryViewModel()
        {
            ApartmentBlock = BlockInitialDataProvider.Generate();
        }

        public ApartmentBlock ApartmentBlock { get => Get<ApartmentBlock>(); set => Set(value); }

        public Flat SelectedFlat { get => Get<Flat>(); set => Set(value); }
    }
}
