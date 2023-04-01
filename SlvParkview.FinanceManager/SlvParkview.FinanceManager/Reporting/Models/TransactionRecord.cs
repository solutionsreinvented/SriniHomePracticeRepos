using ReInvented.Shared.Stores;
using SlvParkview.FinanceManager.Models;
using System;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class TransactionRecord : PropertyStore
    {
        public DateTime TransactionDate { get => Get<DateTime>(); set => Set(value); }

        public Bill Bill { get => Get<Bill>(); set => Set(value); }

        public Receipt Receipt { get => Get<Receipt>(); set => Set(value); }

        public decimal Outstanding { get => Get<decimal>(); set => Set(value); }

        public TransactionInfo TransactionInfo => TransactionInfo.Parse(this);

    }
}
