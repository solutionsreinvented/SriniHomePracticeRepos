using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace DevDrive.Services
{
    public static class AnimationsService
    {
        public static DoubleAnimation CreateDoubleAnimation(double from, double to, double durationSeconds, DependencyProperty targetProperty)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(durationSeconds))
            };
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(targetProperty));
            return doubleAnimation;
        }
    }

    public static class AnimationManager
    {
        public static readonly DependencyProperty LoadWindowAnimationProperty = DependencyProperty.RegisterAttached(
                "LoadWindowAnimation", typeof(bool), typeof(AnimationManager), new PropertyMetadata(false, OnLoadWindowAnimationChanged));

        public static readonly DependencyProperty UnloadWindowAnimationProperty = DependencyProperty.RegisterAttached(
                "UnloadWindowAnimation", typeof(bool), typeof(AnimationManager), new PropertyMetadata(false, OnUnloadWindowAnimationChanged));

        public static bool GetLoadWindowAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(LoadWindowAnimationProperty);
        }

        public static void SetLoadWindowAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(LoadWindowAnimationProperty, value);
        }

        public static bool GetUnloadWindowAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(UnloadWindowAnimationProperty);
        }

        public static void SetUnloadWindowAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(UnloadWindowAnimationProperty, value);
        }

        private static void OnLoadWindowAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window && (bool)e.NewValue)
            {
                Storyboard storyboard = new Storyboard();

                double width = window.Width;
                double height = window.Height;
                window.Opacity = 0;

                DoubleAnimation widthAnimation = AnimationsService.CreateDoubleAnimation(0, width, 0.3, FrameworkElement.WidthProperty);
                DoubleAnimation heightAnimation = AnimationsService.CreateDoubleAnimation(0, height, 0.3, FrameworkElement.HeightProperty);
                DoubleAnimation opacityAnimation = AnimationsService.CreateDoubleAnimation(0, 1, 0.3, UIElement.OpacityProperty);

                //storyboard.Children.Add(widthAnimation);
                //storyboard.Children.Add(heightAnimation);
                storyboard.Children.Add(opacityAnimation);

                window.Loaded += (s, args) => storyboard.Begin(window);
            }
        }

        private static void OnUnloadWindowAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window && (bool)e.NewValue)
            {
                window.Closing += (s, args) =>
                {
                    Storyboard storyboard = new Storyboard();

                    DoubleAnimation widthAnimation = new DoubleAnimation
                    {
                        From = window.Width,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromSeconds(1))
                    };
                    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));

                    DoubleAnimation heightAnimation = new DoubleAnimation
                    {
                        From = window.Height,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromSeconds(1))
                    };
                    Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));

                    DoubleAnimation opacityAnimation = new DoubleAnimation
                    {
                        From = 1,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromSeconds(1))
                    };
                    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                    storyboard.Children.Add(widthAnimation);
                    storyboard.Children.Add(heightAnimation);
                    storyboard.Children.Add(opacityAnimation);

                    storyboard.Begin(window);
                };
            }
        }
    }
}
