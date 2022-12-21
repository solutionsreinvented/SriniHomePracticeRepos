using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum CompletionStatus
    {
        [Description("Completed")]
        Completed,
        [Description("On Track")]
        OnTrack,
        [Description("Rescheduled")]
        Rescheduled,
        [Description("Temporary Hold")]
        TemporaryHold,
        [Description("Permanent Hold")]
        PermanentHold
    }
}
