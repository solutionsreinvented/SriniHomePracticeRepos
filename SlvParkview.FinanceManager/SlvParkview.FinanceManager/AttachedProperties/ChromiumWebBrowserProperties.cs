using System.Windows;

using CefSharp.Wpf;
using CefSharp;

namespace SlvParkview.FinanceManager.AttachedProperties
{
    public static class ChromiumWebBrowserProperties
    {
        #region Html Property
        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        // Using a DependencyProperty as the backing store for Html.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof(string), typeof(ChromiumWebBrowserProperties), new PropertyMetadata(string.Empty, OnHtmlChanged));

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChromiumWebBrowser browser)
            {
                string html = e.NewValue as string;
                browser.LoadHtml(html);
            }
        }
        #endregion

    }
}
