using AllCorePracticeApp.Converters;
using System;
using System.Windows;
using System.Windows.Data;

namespace AllCorePracticeApp.Extensions
{
    public class ThemeResourceExtension : DynamicResourceExtension
    {
        public ConverterType ConverterType { get; set; } = ConverterType.BoolToVisibilityConverter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IValueConverter converter;

            switch (ConverterType)
            {
                case ConverterType.BoolToVisibilityConverter:
                    converter = new BoolToVisibilityConverter();
                    break;
                case ConverterType.BoolToBackgroundConverter:
                    converter = new BoolToBackgroundConverter();
                    break;
                default:
                    converter = new BoolToVisibilityConverter();
                    break;
            }

            return converter;
        }
    }
}
