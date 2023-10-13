using System.ComponentModel;

namespace ReInvented.Domain.Design.ExcelInterop.Enums
{
    /// <summary>
    /// Indicates where the gusset plate is connected on the box section.
    /// </summary>
    public enum GussetToBoxSectionConnectionSide
    {
        [Description("Longer Side")]
        Longer,
        [Description("Shorter Side")]
        Shorter
    }
}
