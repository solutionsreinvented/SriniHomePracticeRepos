using System;
using System.Windows;
using System.Windows.Controls;

namespace PerformanceManager.UI.CustomControls
{
    public class PasswordBoxExtended : Control
    {
        private static PasswordBox _passwordBox;

        static PasswordBoxExtended()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBoxExtended), new FrameworkPropertyMetadata(typeof(PasswordBoxExtended)));
        }

        public override void OnApplyTemplate()
        {
            _passwordBox = Template.FindName("PART_PasswordBox", this) as PasswordBox;

            _passwordBox.PasswordChanged += OnPasswordBoxPasswordChanged;

            base.OnApplyTemplate();
        }

        private void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _passwordBox.Password;
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxExtended), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));


        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ///_passwordBox.Password = d.GetValue(PasswordProperty) as string;
        }
    }
}
