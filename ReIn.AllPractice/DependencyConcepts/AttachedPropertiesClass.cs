using System.Windows;
using System.Windows.Controls;

namespace DependencyConcepts.AttachedProperties
{
    public class AttachedPropertiesClass : DependencyObject
    {
        public static bool GetIsDependent(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDependentProperty);
        }

        public static void SetIsDependent(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDependentProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsDependent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDependentProperty =
            DependencyProperty.RegisterAttached("IsDependent", typeof(bool), typeof(AttachedPropertiesClass), new PropertyMetadata(false, OnIsDependentChanged));

        private static void OnIsDependentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = d as TextBox;

            textBox.GotFocus -= OnTextBoxGotFocus;
            textBox.GotFocus += OnTextBoxGotFocus;
        }

        private static void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.SelectAll();
        }
    }
}
