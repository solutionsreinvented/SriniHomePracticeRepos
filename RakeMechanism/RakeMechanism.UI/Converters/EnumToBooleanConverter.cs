using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RakeMechanism.UI.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            string? checkValue = value.ToString();
            string? targetValue = parameter.ToString();

            return string.Equals(checkValue, targetValue, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isChecked || parameter == null)
            {
                return Binding.DoNothing;
            }

            if (!isChecked)
            {
                return Binding.DoNothing;
            }

            string? enumValue = parameter.ToString();
            return enumValue == null ? Binding.DoNothing : Enum.Parse(targetType, enumValue);
        }
    }
}
