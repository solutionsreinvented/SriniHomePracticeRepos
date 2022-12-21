using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace ReIn.VectorsPractice.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum FrameGridType
    {
        [Description("Portal Frame Grid")]
        PortalFrameGrid,
        [Description("Bridge Frame Grid")]
        BridgeFrameGrid,
        [Description("Extension Frame Grid")]
        ExtensionFrameGrid
    }
}
