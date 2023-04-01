using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.Extensions
{
    public static class ReceiptExtensions
    {
        public static ReceiptInfo ParseToReceiptInfo(this Receipt receipt, Flat flat)
        {
            return new ReceiptInfo()
            {
                Amount = receipt.Amount.FormatNumber("N1"),
                FlatNumber = flat.Description,
                OwnerName = flat.OwnedBy,
                Mode = ConvertersService.ReceiptModeConverter.ConvertToString(receipt.Mode),
                ReceivedOn = receipt.ReceivedOn.ToString("dd-MM-yyyy"),
                Category = ConvertersService.TransactionCategoryConverter.ConvertToString(receipt.Category)
            };
        }
    }
}
