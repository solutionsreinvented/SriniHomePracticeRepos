using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Extensions
{
    public static class PaymentExtensions
    {
        public static PaymentInfo ParseToPaymentInfo(this Payment payment, string flatNumber)
        {
            return new PaymentInfo()
            {
                Amount = payment.Amount.FormatNumber("N2"),
                FlatNumber = flatNumber,
                Mode = payment.Mode.ToString(),
                PaymentReceivedOn = payment.ReceivedOn.ToString("dd MMMM yyyy")
            };
        }
    }
}
