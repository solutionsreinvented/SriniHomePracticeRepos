using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum PaymentsReportType
    {
        [Description("Monthwise")]
        Monthwise,
        [Description("Upto a Selected Date")]
        ToASelectedDate
    }
}
