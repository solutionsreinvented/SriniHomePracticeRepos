using System.Windows;

namespace AllCorePracticeApp.AttachedProperties
{
    public class ButtonProperties
    {


        public static int GetCornerRadius(DependencyObject obj)
        {
            return (int)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, int value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(int), typeof(ButtonProperties), new PropertyMetadata(0));


    }
}
