using ReInvented.Shared.Stores;

using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Models
{
    public class OutstandingsReport : PropertyStore
    {
        public PrintableFlat FlatInfo { get => Get<PrintableFlat>(); set => Set(value); }

        public List<PrintableTransaction> Transactions { get; set; }
    }
}
