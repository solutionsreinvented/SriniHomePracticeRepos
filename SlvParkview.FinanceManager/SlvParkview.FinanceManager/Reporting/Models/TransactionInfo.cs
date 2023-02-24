using ReInvented.Shared.Stores;
using ReInvented.Shared.TypeConverters;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class TransactionInfo : PropertyStore
    {
        private static EnumToDescriptionTypeConverter _enumToDescriptionConverter = new EnumToDescriptionTypeConverter(typeof(TransactionCategory));

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
            ///var expenseCategory = _enumToDescriptionConverter.ConvertTo()
            return new TransactionInfo()
            {
                TransactionDate = record.TransactionDate.ToString("dd MMM yyyy"),
                PaymentAmount = record.Payment != null ? record.Payment.Amount.FormatNumber(numberFormat) : blank,
                PaymentCategory = 
                record.Payment != null ?
                _enumToDescriptionConverter.ConvertTo(record.Payment.Category, typeof(string)).ToString() : blank,
                PaymentMode = record.Payment != null ? record.Payment.Mode.ToString() : blank,
                ExpenseAmount = record.Expense != null ? record.Expense.Amount.FormatNumber(numberFormat) : blank,

                ExpenseCategory = 
                record.Expense != null ? 
                _enumToDescriptionConverter.ConvertTo(record.Expense.Category, typeof(string)).ToString() : blank,

                Outstanding = record.Outstanding.FormatNumber(numberFormat)
            };
        }
    }
}
