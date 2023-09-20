using System;
using System.Windows;
using System.Windows.Media.Animation;

using ReInvented.Animations.Enums;

namespace ReInvented.Animations.Helpers
{
    public class AnimationHelper
    {
        public static AnimationTimeline CreateAnimation(AnimationKind animationKind, double fromValue, double toValue, TimeSpan duration)
        {
            AnimationTimeline animation;

            switch (animationKind)
            {
                case AnimationKind.StretchFromLeft:
                    animation = new DoubleAnimation
                    {
                        From = fromValue,
                        To = toValue,
                        Duration = new Duration(duration)
                    };
                    break;

                case AnimationKind.SqueezeToRight:
                    animation = new DoubleAnimation
                    {
                        From = fromValue,
                        To = toValue,
                        Duration = new Duration(duration)
                    };
                    break;

                default:
                    throw new ArgumentException("Invalid animation kind.");
            }

            return animation;
        }
    }
}
