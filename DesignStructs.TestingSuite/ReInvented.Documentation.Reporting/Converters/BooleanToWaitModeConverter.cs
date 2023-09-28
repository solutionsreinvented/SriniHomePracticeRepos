using System;
using System.Globalization;

using ReInvented.Shared.MarkupExtensions;
using ReInvented.TestingSuite;

namespace ReInvented.Documentation.Reporting.Converters
{
    public class BooleanToWaitModeConverter : ValueConverterMarkupExtension<BooleanToWaitModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (WaitMode)value == WaitMode.Wait;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? WaitMode.Wait : (object)WaitMode.Return;
        }
    }
}
