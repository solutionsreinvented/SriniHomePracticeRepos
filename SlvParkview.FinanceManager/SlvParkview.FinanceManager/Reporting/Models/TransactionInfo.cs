using ReInvented.Shared.Stores;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class TransactionInfo : PropertyStore
    {
        public string TransactionDate { get => Get<string>(); set => Set(value); }

        public string PaymentAmount { get => Get<string>(); set => Set(value); }

        public string PaymentMode { get => Get<string>(); set => Set(value); }

        public string PaymentCategory { get => Get<string>(); set => Set(value); }

        public string ExpenseAmount { get => Get<string>(); set => Set(value); }

        public string ExpenseCategory { get => Get<string>(); set => Set(value); }

        public string Outstanding { get => Get<string>(); set => Set(value); }

        public static TransactionInfo Parse(TransactionRecord record)
        {
            string blank = "-";
            string numberFormat = "N1";

            return new TransactionInfo()
            {
                TransactionDate = record.TransactionDate.ToString("dd MMM yyyy"),
                PaymentAmount = record.Payment != null ? record.Payment.Amount.FormatNumber(numberFormat) : blank,

                PaymentCategory = 
                record.Payment != null ?
                ConvertersService.TransactionCategoryConverter.ConvertToString(record.Payment.Category) : blank,

                PaymentMode = 
                record.Payment != null ?
                ConvertersService.PaymentModeConverter.ConvertToString(record.Payment.Mode) : blank,

                ExpenseAmount = record.Expense != null ? record.Expense.Amount.FormatNumber(numberFormat) : blank,

                ExpenseCategory = 
                record.Expense != null ?
                ConvertersService.TransactionCategoryConverter.ConvertToString(record.Expense.Category) : blank,

                Outstanding = record.Outstanding.FormatNumber(numberFormat)
            };
        }
    }
}
