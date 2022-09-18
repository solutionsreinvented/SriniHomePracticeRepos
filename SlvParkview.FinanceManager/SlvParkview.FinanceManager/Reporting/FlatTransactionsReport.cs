using ReInvented.Shared.Stores;
using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Reporting
{
    public class FlatTransactionsReport : Report
    {
        public FlatInfo FlatInfo { get => Get<FlatInfo>(); set => Set(value); }

        public List<TransactionInfo> Transactions { get; set; }

        public override void Generate()
        {
            throw new NotImplementedException();
        }

        private protected override void CreateHtmlFile()
        {
            throw new NotImplementedException();
        }

        private protected override void CreateJsonFile()
        {
            throw new NotImplementedException();
        }

        private protected override void CreateRequiredDirectories()
        {
            throw new NotImplementedException();
        }
    }
}
