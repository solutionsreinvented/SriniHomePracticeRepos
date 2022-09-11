using System;
using System.Globalization;
using System.Windows.Data;

namespace ApartmentFinanceManager.Converters
{
    public class DateToDateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= DateTime.Today;

            return DateOnly.FromDateTime((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= DateTime.Today;

            return (DateTime)value;
        }
    }
}
