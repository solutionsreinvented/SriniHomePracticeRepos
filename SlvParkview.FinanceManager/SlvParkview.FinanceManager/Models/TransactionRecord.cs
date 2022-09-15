using ReInvented.Shared.Stores;

using System;

namespace SlvParkview.FinanceManager.Models
{
    public class TransactionRecord : PropertyStore
    {
        public DateTime TransactionDate { get => Get<DateTime>(); set => Set(value); }

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public Payment Payment { get => Get<Payment>(); set => Set(value); }

        public decimal Outstanding { get => Get<decimal>(); set => Set(value); }

        public PrintableTransaction PrintableTransaction => PrintableTransaction.Parse(this);

    }
}
