using ApartmentFinanceManager.Models;

namespace ApartmentFinanceManager.ViewModels
{
    public class AddPaymentViewModel : BaseViewModel
    {
        public AddPaymentViewModel()
        {
            Payment = new Payment();
        }

        public Payment Payment { get; set; }
    }

}
