using System;
using System.Windows;
using System.Windows.Controls;

namespace SRi.CustomControlsLibrary
{

    public class PasswordBoxEx : Control
    {
        private static PasswordBox _passwordBox;

        static PasswordBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBoxEx), new FrameworkPropertyMetadata(typeof(PasswordBoxEx)));
        }

        public override void OnApplyTemplate()
        {
            _passwordBox = Template.FindName("PART_PasswordBox", this) as PasswordBox;

            _passwordBox.PasswordChanged += OnPasswordBoxPasswordChanged;

            base.OnApplyTemplate();
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxEx), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _passwordBox.Password;
        }



        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        // Using a DependencyProperty as the backing store for PasswordChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(PasswordBoxEx), new PropertyMetadata());

    }
}
