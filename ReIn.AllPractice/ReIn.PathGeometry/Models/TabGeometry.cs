using System.Windows;

namespace ReIn.TabPathGeometry.Models
{
    public class TabGeometry : DependencyObject
    {
        public TabGeometry()
        {

        }



        public double H
        {
            get => (double)GetValue(HProperty);
            set => SetValue(HProperty, value);
        }

        /// Using a DependencyProperty as the backing store for H.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register("H", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0));



        public double B
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        // Using a DependencyProperty as the backing store for B.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0));



    }
}
