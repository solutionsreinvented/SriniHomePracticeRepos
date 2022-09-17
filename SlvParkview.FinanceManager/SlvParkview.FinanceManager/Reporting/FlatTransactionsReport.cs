using ReInvented.Shared.Stores;
using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Reporting
{
    public class FlatTransactionsReport : Report
    {
        public FlatInfo FlatInfo { get => Get<FlatInfo>(); set => Set(value); }

        public List<TransactionInfo> Transactions { get; set; }
    }
}
