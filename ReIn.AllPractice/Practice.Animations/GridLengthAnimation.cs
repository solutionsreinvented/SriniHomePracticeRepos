using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Practice.Animations
{
    public class GridLengthAnimation : AnimationTimeline
    {
        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore() => new GridLengthAnimation();



        public GridLength From
        {
            get { return (GridLength)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        // Using a DependencyProperty as the backing store for From.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation), new PropertyMetadata(null));



        public GridLength To
        {
            get { return (GridLength)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for To.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation), new PropertyMetadata(null));


        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = ((GridLength)GetValue(FromProperty)).Value;
            if (fromValue == 1)
            {
                fromValue = ((GridLength)defaultDestinationValue).Value;
            }
            double toValue = ((GridLength)GetValue(ToProperty)).Value;
            if (fromValue > toValue)
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromValue - toValue) + toValue, GridUnitType.Star);
            }
            else
            {
                return new GridLength(animationClock.CurrentProgress.Value * (toValue - fromValue) + fromValue, GridUnitType.Star);
            }
        }

    }
}
