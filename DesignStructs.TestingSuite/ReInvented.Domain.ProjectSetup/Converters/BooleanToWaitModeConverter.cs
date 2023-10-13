using System;
using System.Globalization;

using ReInvented.Shared.MarkupExtensions;
using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.ProjectSetup.Converters
{
    public class BooleanToWaitModeConverter : ValueConverterMarkupExtension<BooleanToWaitModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (WaitAnalysisMode)value == WaitAnalysisMode.Wait;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? WaitAnalysisMode.Wait : (object)WaitAnalysisMode.Return;
        }
    }
}
