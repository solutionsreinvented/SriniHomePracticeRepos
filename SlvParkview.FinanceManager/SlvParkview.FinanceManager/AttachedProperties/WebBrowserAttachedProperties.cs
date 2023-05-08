using System;
using System.Windows;
using System.Windows.Controls;

namespace SlvParkview.FinanceManager.AttachedProperties
{
    public static class WebBrowserAttachedProperties
    {

        public static readonly DependencyProperty HtmlFilePathProperty =
        DependencyProperty.RegisterAttached("HtmlFilePath", typeof(string), typeof(WebBrowserAttachedProperties), new PropertyMetadata(null, OnHtmlFilePathChanged));

        public static string GetHtmlFilePath(WebBrowser webBrowser)
        {
            return (string)webBrowser.GetValue(HtmlFilePathProperty);
        }

        public static void SetHtmlFilePath(WebBrowser webBrowser, string value)
        {
            webBrowser.SetValue(HtmlFilePathProperty, value);
        }

        private static void OnHtmlFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebBrowser webBrowser && e.NewValue is string htmlFilePath)
            {
                webBrowser.Source = new Uri(htmlFilePath);
            }
        }
    }
}
