using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum ReportType
    {
        [Description("Outstandings Summary")]
        OutstandingsSummary,
        [Description("Flat Transactions Summary")]
        FlatTransactionsSummary
    }
}
