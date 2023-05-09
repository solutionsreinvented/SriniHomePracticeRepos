using System;
using System.Windows;

using Microsoft.Web.WebView2.Wpf;

namespace SlvParkview.FinanceManager.AttachedProperties
{
    public static class WebView2Properties
    {
        #region NullableSource
        [AttachedPropertyBrowsableForType(typeof(WebView2))]
        public static Uri GetNullableSource(DependencyObject obj)
        {
            return (Uri)obj.GetValue(NullableSourceProperty);
        }

        public static void SetNullableSource(DependencyObject obj, Uri value)
        {
            obj.SetValue(NullableSourceProperty, value);
        }

        /// <summary>
        /// Middle man for binding <see cref="WebView2.Source"/> when the binding might resolve to null.
        /// </summary>
        /// <remarks>
        /// <see cref="WebView2.Source"/> can become null when <see cref="WebView2"/> is used in a <see cref="ItemsControl.ItemTemplate"/>. When an item is removed from the <see cref="ItemsControl.ItemsSource"/>, the Binding to <see cref="WebView2.Source"/> will resolve to null prior to the control being unloaded.
        /// </remarks>
        public static readonly DependencyProperty NullableSourceProperty = DependencyProperty.RegisterAttached("NullableSource", typeof(Uri), typeof(WebView2Properties), new PropertyMetadata(propertyChangedCallback: NullableSourceChanged));
        private static void NullableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebView2 webView2)
            {
                webView2.Source = e.NewValue as Uri ?? new Uri("about:blank");
            }
        }
        #endregion

        #region DisposeOnUnloaded
        [AttachedPropertyBrowsableForType(typeof(WebView2))]
        public static bool GetDisposeOnUnloaded(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisposeOnUnloadedProperty);
        }

        public static void SetDisposeOnUnloaded(DependencyObject obj, bool value)
        {
            obj.SetValue(DisposeOnUnloadedProperty, value);
        }

        /// <summary>
        /// When true, disposes <see cref="WebView2"/> when <see cref="FrameworkElement.Unloaded"/> is fired.
        /// </summary>
        /// <remarks>
        /// By default, <see cref="WebView2"/> does not dispose of its resources when its unloaded, which might be desirable in certain situations.
        /// </remarks>
        public static readonly DependencyProperty DisposeOnUnloadedProperty = DependencyProperty.RegisterAttached("DisposeOnUnloaded", typeof(bool), typeof(WebView2Properties), new PropertyMetadata(false, propertyChangedCallback: DisposeOnUnloadedChanged));

        private static void DisposeOnUnloadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is WebView2 webView2))
            {
                return;
            }
            else
            {
                //always attempt to unsubscribe in case this get triggered for "true" multiple times
                webView2.Unloaded -= OnWebView2Unloaded;

                if (e.NewValue is true)
                {
                    webView2.Unloaded += OnWebView2Unloaded;
                }
            }
        }

        private static void OnWebView2Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is WebView2 webView2)
            {
                webView2.Dispose();
                webView2.Unloaded -= OnWebView2Unloaded;
            }
        }
        #endregion
    }
}
