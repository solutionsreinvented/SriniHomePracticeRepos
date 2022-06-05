using System;
using System.Windows;

namespace AllCorePracticeApp.AttachedProperties
{
    public abstract class AttachedPropertyBase<TParent, TProperty> where TParent : AttachedPropertyBase<TParent, TProperty>, new()
    {

        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        public static TParent Instance { get; private set; } = new TParent();

        public static TProperty GetValue(DependencyObject obj)
        {
            return (TProperty)obj.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject obj, TProperty value)
        {
            obj.SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(TProperty), typeof(TParent), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Instance.ValueChanged(d, e);
        }

        public virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }

    }
}
