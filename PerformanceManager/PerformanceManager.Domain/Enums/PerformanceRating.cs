using PerformanceManager.Domain.Converters;

using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PerformanceRating
    {
        NeedsSignificantImprovement = 1,
        NeedsImprovement = 2,
        Satisfactory = 3,
        VerySatisfactory = 4,
        Outstanding = 5,
    }
}
