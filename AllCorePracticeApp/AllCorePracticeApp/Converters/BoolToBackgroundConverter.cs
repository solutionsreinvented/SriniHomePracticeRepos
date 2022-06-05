using System;
using System.Globalization;
using System.Windows.Media;

namespace AllCorePracticeApp.Converters
{
    public class BoolToBackgroundConverter : ConverterMarkupExtension<BoolToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new SolidColorBrush(Colors.PowderBlue) : new SolidColorBrush(Colors.SandyBrown);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
