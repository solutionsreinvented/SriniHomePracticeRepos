using PerformanceManager.Domain.Converters;

using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Discipline
    {
        Design,
        Detailing,
        Development
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
}
