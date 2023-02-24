using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.Extensions
{
    public static class PaymentExtensions
    {
        public static PaymentInfo ParseToPaymentInfo(this Payment payment, Flat flat)
        {
            return new PaymentInfo()
            {
                Amount = payment.Amount.FormatNumber("N1"),
                FlatNumber = flat.Description,
                OwnerName = flat.OwnedBy,
                Mode = ConvertersService.PaymentModeConverter.ConvertTo(payment.Mode, typeof(string)).ToString(),
                ReceivedOn = payment.ReceivedOn.ToString("dd-MM-yyyy"),
                Category = ConvertersService.TransactionCategoryConverter.ConvertTo(payment.Category, typeof(string)).ToString()
            };
        }
    }
}
