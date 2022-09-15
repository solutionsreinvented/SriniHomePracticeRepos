using ReInvented.Shared.Stores;

using System;

namespace SlvParkview.FinanceManager.Models
{
    public class PrintableTransaction : PropertyStore
    {
        public DateTime TransactionDate { get => Get<DateTime>(); set => Set(value); }

        public string PaymentAmount { get => Get<string>(); set => Set(value); }

        public string PaymentMode { get => Get<string>(); set => Set(value); }

        public string PaymentCategory { get => Get<string>(); set => Set(value); }

        public string ExpenseAmount { get => Get<string>(); set => Set(value); }

        public string ExpenseCategory { get => Get<string>(); set => Set(value); }

        public string Outstanding { get => Get<string>(); set => Set(value); }

        public static PrintableTransaction Parse(TransactionRecord record)
        {
            string blank = "-";

            return new PrintableTransaction()
            {
                TransactionDate = record.TransactionDate,
                PaymentAmount = record.Payment != null ? record.Payment.Amount.ToString() : blank,
                PaymentCategory = record.Payment != null ? record.Payment.Category.ToString() : blank,
                PaymentMode = record.Payment != null ? record.Payment.Mode.ToString() : blank,
                ExpenseAmount = record.Expense != null ? record.Expense.Amount.ToString() : blank,
                ExpenseCategory = record.Expense != null ? record.Expense.Category.ToString() : blank,
                Outstanding = record.Outstanding.ToString()
            };
        }
    }
}
