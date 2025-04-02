using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace ReInvented.UniformMesher.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum Direction
    {
        [Description("Circumferential")]
        Circumferential,
        [Description("Radial")]
        Radial,
        [Description("Vertical")]
        Vertical
    }
}
