using ActivityTracker.Domain.Converters;

using System.ComponentModel;

namespace ActivityTracker.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Discipline
    {
        Design = 11,
        Detailing = 15,
        Development = 17
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
