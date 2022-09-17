using ReInvented.Shared.Stores;
using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Reporting
{
    public class OverviewReport : Report, IReport
    {
        public List<FlatInfo> Flats { get => Get<List<FlatInfo>>(); set => Set(value); }

    }
}
