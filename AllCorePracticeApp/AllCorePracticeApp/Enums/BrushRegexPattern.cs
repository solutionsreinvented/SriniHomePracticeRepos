using System.ComponentModel;

namespace AllCorePracticeApp.Enums
{
    public enum BrushRegexPattern
    {
        /// The RegEx used in this description is partial and only checks for integer values 0-360 with word boundaries
        /// and also does not omit the negative values. So this needs to updated to cater for gradient stops and fractional portion of the angle.
        [Description(@"\b(\d|\d{1,2}|[0-2]\d{1,2}|3(?:[0-5]\d|60))\b")]
        GradientStopsWithAngle,

        [Description(@"(#[0-9,A-F]{6,8}\s(?:0*(?:\.\d+)|1(?:\.0*)))+")]
        GradientStopsOnly,

        ///[Description(@"(#[0-9,A-F]{6,8} [0-1]\.[0-9]+)+")]
        GradientStopsWithStartAndEndPoints,
        [Description(@"^#(?:[A-F0-9]{6}|[A-F0-9]{8})$")]
        SolidColor
    }
}
