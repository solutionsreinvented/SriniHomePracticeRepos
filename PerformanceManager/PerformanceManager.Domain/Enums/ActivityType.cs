using PerformanceManager.Domain.Converters;

using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ActivityType
    {
        Design,
        Detailing
    }
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DesignSubActivityType
    {
        Tank,
        TankAndSupportStructure,
        Bridge,
        TankSupportStructureAndBridge,
        RakeMechanism,
        CompleteThickener
    }
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DetailingSubActivityType
    {
        SupportStructure,
        Bridge,
        RakeMechanism,
    }
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
