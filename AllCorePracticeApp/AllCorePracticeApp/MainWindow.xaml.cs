using AllCorePracticeApp.Enums;
using ReInvented.Shared;
using SRi.XamlUIThickenerApp.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace AllCorePracticeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GenerateGradientBrush();

            var patternCollection = GetPatternsCollection();

        }

        private Dictionary<GradientRegexPattern, string> GetPatternsCollection()
        {
            var patternCollection = new Dictionary<GradientRegexPattern, string>();

            foreach (GradientRegexPattern patternKey in Enum.GetValues(typeof(GradientRegexPattern)))
            {
                string patternDescription = patternKey.GetDescription();
                patternCollection.Add(patternKey, patternDescription);
            }

            return patternCollection;
        }

        private void GenerateGradientBrush()
        {
            Dictionary<ResourceKey, Brush> brushDictionary;

            var path = @"C:\Users\srini\source\repos\AllCorePracticeApp\AllCorePracticeApp\Json\Default.json";

            var gradientStops = new GradientStopCollection()
            {
                new GradientStop(Colors.AliceBlue, 0.5),
                new GradientStop(Colors.Red, 1),
            };

            var linearGradientBrush = new LinearGradientBrush(gradientStops);

            JsonDataSerializer<Dictionary<string, string>> serializer = new JsonDataSerializer<Dictionary<string, string>>();

            var deserialized = serializer.Deserialiaze(path);

            brushDictionary = new Dictionary<ResourceKey, Brush>();

            foreach (var key in deserialized.Keys)
            {
                brushDictionary.Add((ResourceKey)Enum.Parse(typeof(ResourceKey), key), ParseStringToBrush(deserialized[key]));
            }

            Background = brushDictionary[ResourceKey.DisabledForeground];

        }

        private Brush ParseStringToBrush(string colorString)
        {
            Brush brush;

            if (colorString.Where(c => c == '#').Count() == 1)
            {
                var color = (Color?)ColorConverter.ConvertFromString(colorString);

                brush = new SolidColorBrush(color != null ? color.Value : Colors.Transparent);
            }
            else
            {
                var patterns = GetPatternsCollection();

                var gradientType = patterns.Values.FirstOrDefault(v => Regex.IsMatch(colorString, v));
                var key = patterns.Keys.FirstOrDefault(k => patterns[k] == gradientType);
                brush = ParseStringToLinearGradientBrush(colorString);
            }

            return brush;
        }

        private Brush ParseStringToLinearGradientBrush(string colorString)
        {
            string gradientStopPattern = @"(#[0-9,A-F]{6,8}\s(?:0*(?:\.\d+)|1(?:\.0*)))+";

            LinearGradientBrush brush;

            if (Regex.IsMatch(colorString, gradientStopPattern))
            {
                GradientStopCollection gradienStops = GetGradientStops(colorString, gradientStopPattern);

                brush = new LinearGradientBrush(gradienStops);
                ///new LinearGradientBrush()
            }
            else
            {
                brush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop(Colors.Red, 0), new GradientStop(Colors.Blue, 1) });
            }

            return brush;
        }

        private static GradientStopCollection GetGradientStops(string colorString, string gradientStopPattern)
        {
            var colorOffsetPairs = Regex.Split(colorString, gradientStopPattern).Where(s => !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, gradientStopPattern));
            var gradienStops = new GradientStopCollection();

            colorOffsetPairs.ToList().ForEach(cd => gradienStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(cd.Split(' ')[0]), double.Parse(cd.Split(' ')[1]))));
            return gradienStops;
        }
    }

    enum ResourceKey
    {
        StaticBackground,
        MouseOverBackground,
        PressedBackground,
        DisabledBackground,
        StaticForeground,
        MouseOverForeground,
        PressedForeground,
        DisabledForeground
    }


}
