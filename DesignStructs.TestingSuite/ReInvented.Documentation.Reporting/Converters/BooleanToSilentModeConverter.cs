﻿using System;
using System.Globalization;

using ReInvented.Shared.MarkupExtensions;
using ReInvented.TestingSuite;

namespace ReInvented.Documentation.Reporting.Converters
{
    public class BooleanToSilentModeConverter : ValueConverterMarkupExtension<BooleanToSilentModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SilentMode)value == SilentMode.Enable;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? SilentMode.Enable : (object)SilentMode.Disable;
        }
    }
}