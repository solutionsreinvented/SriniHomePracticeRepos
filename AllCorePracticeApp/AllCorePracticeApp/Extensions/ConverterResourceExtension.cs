using AllCorePracticeApp.Converters;
using AllCorePracticeApp.Enums;
using System;
using System.Windows;
using System.Windows.Data;

namespace AllCorePracticeApp.Extensions
{
    public class ConverterResourceExtension : DynamicResourceExtension
    {
        public ConverterType Type { get; set; } = ConverterType.BoolToVisibilityConverter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IValueConverter converter;

            switch (Type)
            {
                case ConverterType.BoolToVisibilityConverter:
                    converter = new BoolToVisibilityConverter();
                    break;
                case ConverterType.BoolToBackgroundConverter:
                    converter = new BoolToBackgroundConverter();
                    break;
                case ConverterType.DesignCodeToViewConverter:
                    converter = new DesignCodeToViewConverter();
                    break;
                default:
                    converter = new BoolToVisibilityConverter();
                    break;
            }

            return converter;
        }
    }
}
