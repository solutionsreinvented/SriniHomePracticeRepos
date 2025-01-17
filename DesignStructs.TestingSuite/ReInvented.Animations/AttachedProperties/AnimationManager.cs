using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ReInvented.Animations.AttachedProperties
{
    public static class AnimationManager
    {
        // Attached property for Load Animation
        public static readonly DependencyProperty LoadWidthAnimationProperty =
            DependencyProperty.RegisterAttached("LoadWidthAnimation", typeof(bool), typeof(AnimationManager),
                new PropertyMetadata(false, OnLoadWidthAnimationChanged));

        public static bool GetLoadWidthAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(LoadWidthAnimationProperty);
        }

        public static void SetLoadWidthAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(LoadWidthAnimationProperty, value);
        }

        private static void OnLoadWidthAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window && (bool)e.NewValue)
            {
                window.Loaded += (s, args) =>
                {
                    AnimateWindowWidth(window);
                };
            }
        }

        private static void AnimateWindowWidth(Window window)
        {
            // Save the original width and set the initial width to 0
            double targetWidth = window.Width;
            window.Width = 0;
            // Create the width animation
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = targetWidth,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            // Apply the animation
            window.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            window.BeginAnimation(FrameworkElement.OpacityProperty, opacityAnimation);
        }
    }
}
