using System;
using System.Linq;
using System.Windows;

namespace ProdActivity.UI.Themes
{
    public static class ThemeManager
    {
        private const string LightThemeUri = "pack://application:,,,/ProdActivity.UI;component/Themes/LightTheme.xaml";
        private const string DarkThemeUri = "pack://application:,,,/ProdActivity.UI;component/Themes/DarkTheme.xaml";

        public static bool IsDarkTheme { get; private set; } = true;

        public static void ToggleTheme()
        {
            var appResources = Application.Current.Resources.MergedDictionaries;
            
            var existingTheme = appResources.FirstOrDefault(d => 
                d.Source != null && 
                (d.Source.ToString().Contains("LightTheme.xaml") || d.Source.ToString().Contains("DarkTheme.xaml")));

            if (existingTheme != null)
            {
                appResources.Remove(existingTheme);
            }

            IsDarkTheme = !IsDarkTheme;
            string targetThemeUri = IsDarkTheme ? DarkThemeUri : LightThemeUri;

            var newTheme = new ResourceDictionary { Source = new Uri(targetThemeUri, UriKind.Absolute) };
            appResources.Add(newTheme);
        }

        public static void SetTheme(bool useDarkTheme)
        {
            if (IsDarkTheme != useDarkTheme)
            {
                ToggleTheme();
            }
        }
    }
}
