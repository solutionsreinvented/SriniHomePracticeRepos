using System.ComponentModel;

namespace AllCorePracticeApp.Enums
{
    public enum GradientRegexPattern
    {
        [Description(@"Random")]
        GradientStopsWithAngle,

        [Description(@"(#[0-9,A-F]{6,8}\s(?:0*(?:\.\d+)|1(?:\.0*)))+")]
        GradientStopsOnly,

        ///[Description(@"(#[0-9,A-F]{6,8} [0-1]\.[0-9]+)+")]
        GradientStopsWithStartAndEndPoint
    }
}
