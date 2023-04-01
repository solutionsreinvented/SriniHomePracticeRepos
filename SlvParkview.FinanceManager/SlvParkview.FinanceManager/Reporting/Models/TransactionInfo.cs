using ReInvented.Shared.Stores;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class TransactionInfo : PropertyStore
    {
        public string TransactionDate { get => Get<string>(); set => Set(value); }

        public string ReceiptAmount { get => Get<string>(); set => Set(value); }

        public string ReceiptMode { get => Get<string>(); set => Set(value); }

        public string ReceiptCategory { get => Get<string>(); set => Set(value); }

        public string BillAmount { get => Get<string>(); set => Set(value); }

        public string BillCategory { get => Get<string>(); set => Set(value); }

        public string Outstanding { get => Get<string>(); set => Set(value); }

        public static TransactionInfo Parse(TransactionRecord record)
        {
            string blank = "-";
            string numberFormat = "N1";

            return new TransactionInfo()
            {
                TransactionDate = record.TransactionDate.ToString("dd MMM yyyy"),
                ReceiptAmount = record.Receipt != null ? record.Receipt.Amount.FormatNumber(numberFormat) : blank,

                ReceiptCategory =
                record.Receipt != null ?
                ConvertersService.TransactionCategoryConverter.ConvertToString(record.Receipt.Category) : blank,

                ReceiptMode =
                record.Receipt != null ?
                ConvertersService.ReceiptModeConverter.ConvertToString(record.Receipt.Mode) : blank,

                BillAmount = record.Bill != null ? record.Bill.Amount.FormatNumber(numberFormat) : blank,

                BillCategory =
                record.Bill != null ?
                ConvertersService.TransactionCategoryConverter.ConvertToString(record.Bill.Category) : blank,

                Outstanding = record.Outstanding.FormatNumber(numberFormat)
            };
        }
    }
}
