using System.Windows;
using System.Windows.Controls;

namespace ActivityTracker.CustomControls.AttachedProperties
{
    public class PasswordBoxProperties
    {
        #region Properties

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxProperties), new PropertyMetadata(string.Empty, OnPasswordChanged));

        // Using a DependencyProperty as the backing store for IsBindingRequested.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBindingRequestedProperty =
            DependencyProperty.RegisterAttached("IsBindingRequested", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false, OnIsBindingRequestedChanged));

        private static readonly DependencyProperty IsPasswordUpdatingProperty =
            DependencyProperty.RegisterAttached("IsPasswordUpdating", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false));

        #endregion

        #region Event Handlers

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            bool isBindingRequested = GetIsBindingRequested(box);

            // Only handle this event when the property is attached to a PasswordBox and when the IsBindingRequested is set to true.
            if (box == null || !isBindingRequested)
            {
                return;
            }

            // Avoid recursive updating by ignoring the password box's changed event.
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            bool isPasswordUpdating = GetIsPasswordUpdating(box);

            if (!isPasswordUpdating)
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnIsBindingRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When the IsBindingRequested attached property is set on a PasswordBox, start listening to it's PasswordChanged event.

            if (!(d is PasswordBox box))
            {
                return;
            }

            bool boundPreviously = (bool)e.OldValue;
            bool bindingRequested = (bool)e.NewValue;

            if (boundPreviously)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (bindingRequested)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            // Set a flag to indicate that we are updating the password
            SetIsPasswordUpdating(box, true);

            // Push the new password into the BoundPassword property
            SetPassword(box, box.Password);

            SetIsPasswordUpdating(box, false);
        }

        #endregion

        #region CLR Wrappers

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        public static bool GetIsBindingRequested(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBindingRequestedProperty);
        }

        public static void SetIsBindingRequested(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBindingRequestedProperty, value);
        }


        private static void SetIsPasswordUpdating(DependencyObject d, bool value)
        {
            d.SetValue(IsPasswordUpdatingProperty, value);
        }

        private static bool GetIsPasswordUpdating(DependencyObject d)
        {
            return (bool)d.GetValue(IsPasswordUpdatingProperty);
        }

        #endregion
    }
}
