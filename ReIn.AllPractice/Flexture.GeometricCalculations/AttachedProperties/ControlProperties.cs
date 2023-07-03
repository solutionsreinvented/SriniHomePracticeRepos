using System;
using System.Windows;
using System.Windows.Media;

namespace Flexture.GeometricCalculations.AttachedProperties
{
    public static class ControlProperties
    {


        public static GradientBrushProperties GetGradientBrushProperties(DependencyObject obj)
        {
            return (GradientBrushProperties)obj.GetValue(GradientBrushPropertiesProperty);
        }

        public static void SetGradientBrushProperties(DependencyObject obj, GradientBrushProperties value)
        {
            obj.SetValue(GradientBrushPropertiesProperty, value);
        }

        // Using a DependencyProperty as the backing store for GradientBrushProperties.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GradientBrushPropertiesProperty =
            DependencyProperty.RegisterAttached("GradientBrushProperties", typeof(GradientBrushProperties), typeof(ControlProperties), new PropertyMetadata(null, OnGradientBrushPropertiesChanged));

        private static void OnGradientBrushPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class GradientBrushProperties : DependencyObject
    {


        public Color Start
        {
            get { return (Color)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Start.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(Color), typeof(GradientBrushProperties), new PropertyMetadata(Colors.Transparent));



        public Color End
        {
            get { return (Color)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for End.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(Color), typeof(GradientBrushProperties), new PropertyMetadata(Colors.Transparent));


    }

}
