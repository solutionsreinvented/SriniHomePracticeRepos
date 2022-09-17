using ReInvented.Shared.Stores;
using SlvParkview.FinanceManager.Models;
using System;

namespace SlvParkview.FinanceManager.Reporting
{
    public class TransactionRecord : PropertyStore
    {
        public DateTime TransactionDate { get => Get<DateTime>(); set => Set(value); }

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public Payment Payment { get => Get<Payment>(); set => Set(value); }

        public decimal Outstanding { get => Get<decimal>(); set => Set(value); }

        public TransactionInfo TransactionInfo => TransactionInfo.Parse(this);

    }
}
