using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum ReportType
    {
        [Description("Flat Transactions History")]
        FlatTransactionsHistory,
        [Description("Block Outstandings")]
        BlockOutstandings,
        [Description("Payments in a Month")]
        PaymentsInAMonth,
    }
}
