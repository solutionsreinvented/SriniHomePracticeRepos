using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AllCorePracticeApp.Converters
{
    /// <summary>
    /// Value converter combined with the capabilities of the Markup Extension in order to reduce the overhead of creating converters manually.
    /// </summary>
    /// <typeparam name="TConverter">Type of the value converter that is getting created.</typeparam>
    public abstract class ConverterMarkupExtension<TConverter> : MarkupExtension, IValueConverter where TConverter : class, new()
    {
        private static TConverter converter = null;

        #region Markup Extension Functions

        public override object ProvideValue(IServiceProvider serviceProvider) => converter ??= new TConverter();

        #endregion

        #region Value Converter Functions

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion

    }
}
