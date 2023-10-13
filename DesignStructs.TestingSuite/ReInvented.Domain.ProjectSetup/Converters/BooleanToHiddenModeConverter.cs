using System;
using System.Globalization;

using ReInvented.Shared.MarkupExtensions;
using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.ProjectSetup.Converters
{
    public class BooleanToHiddenModeConverter : ValueConverterMarkupExtension<BooleanToHiddenModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (HiddenMode)value == HiddenMode.Enable;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? HiddenMode.Enable : (object)HiddenMode.Disable;
        }
    }
}
