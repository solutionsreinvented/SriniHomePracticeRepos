using System;
using System.Windows;
using System.Windows.Media.Animation;

using ReInvented.Animations.Enums;
using ReInvented.Animations.Helpers;

namespace ReInvented.Animations.AttachedProperties
{
    public class AnimationProperties
    {

        #region ControlLoadAnimationProperty

        public static readonly DependencyProperty ControlLoadAnimationProperty = DependencyProperty.RegisterAttached("ControlLoadAnimation", typeof(AnimationKind), typeof(AnimationProperties), new PropertyMetadata(AnimationKind.StretchFromLeft, OnControlLoadAnimationChanged));

        public static AnimationKind GetControlLoadAnimation(DependencyObject obj)
        {
            return (AnimationKind)obj.GetValue(ControlLoadAnimationProperty);
        }

        public static void SetControlLoadAnimation(DependencyObject obj, AnimationKind value)
        {
            obj.SetValue(ControlLoadAnimationProperty, value);
        }

        private static void OnControlLoadAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && e.NewValue is AnimationKind animationKind)
            {
                AnimationTimeline animation = AnimationHelper.CreateAnimation(animationKind, 0, 1, TimeSpan.FromSeconds(0.5));

                // Set the animation on the element
                element.BeginAnimation(FrameworkElement.WidthProperty, animation);
            }
        }

        #endregion

        #region AnimationKindProperty

        public static readonly DependencyProperty AnimationKindProperty =
            DependencyProperty.RegisterAttached("AnimationKind", typeof(AnimationKind), typeof(AnimationProperties), new PropertyMetadata(AnimationKind.StretchFromLeft, OnAnimationKindChanged));

        public static AnimationKind GetAnimationKind(DependencyObject obj)
        {
            return (AnimationKind)obj.GetValue(AnimationKindProperty);
        }

        public static void SetAnimationKind(DependencyObject obj, AnimationKind value)
        {
            obj.SetValue(AnimationKindProperty, value);
        }

        private static void OnAnimationKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.NewValue is AnimationKind animationKind)
            {
                AnimationTimeline animation = AnimationHelper.CreateAnimation(animationKind, 0, 1, TimeSpan.FromSeconds(0.5));

                // Set the animation on the element
                element.BeginAnimation(FrameworkElement.WidthProperty, animation);
            }
        }

        #endregion

        #region LoadAnimationKindProperty and UnloadAnimationKindProperty

        public static readonly DependencyProperty LoadAnimationKindProperty =
            DependencyProperty.RegisterAttached("LoadAnimationKind", typeof(AnimationKind), typeof(AnimationProperties), new PropertyMetadata(AnimationKind.SqueezeToRight, OnLoadAnimationKindChanged));

        public static readonly DependencyProperty UnloadAnimationKindProperty =
            DependencyProperty.RegisterAttached("UnloadAnimationKind", typeof(AnimationKind), typeof(AnimationProperties), new PropertyMetadata(AnimationKind.StretchFromLeft, OnUnloadAnimationKindChanged));

        public static AnimationKind GetLoadAnimationKind(DependencyObject obj)
        {
            return (AnimationKind)obj.GetValue(LoadAnimationKindProperty);
        }

        public static void SetLoadAnimationKind(DependencyObject obj, AnimationKind value)
        {
            obj.SetValue(LoadAnimationKindProperty, value);
        }

        public static AnimationKind GetUnloadAnimationKind(DependencyObject obj)
        {
            return (AnimationKind)obj.GetValue(UnloadAnimationKindProperty);
        }

        public static void SetUnloadAnimationKind(DependencyObject obj, AnimationKind value)
        {
            obj.SetValue(UnloadAnimationKindProperty, value);
        }

        private static void OnLoadAnimationKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.NewValue is AnimationKind animationKind)
            {
                AnimationTimeline animation = AnimationHelper.CreateAnimation(animationKind, 0, element.DesiredSize.Width, TimeSpan.FromSeconds(0.5));

                // Set the animation on the element
                element.Loaded += (sender, args) =>
                {
                    element.BeginAnimation(FrameworkElement.WidthProperty, animation);
                };
            }
        }

        private static void OnUnloadAnimationKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.OldValue is AnimationKind animationKind)
            {
                AnimationTimeline animation = AnimationHelper.CreateAnimation(animationKind, element.DesiredSize.Width, 0, TimeSpan.FromSeconds(0.5));

                // Set the animation on the element
                element.Unloaded += (sender, args) =>
                {
                    element.BeginAnimation(FrameworkElement.WidthProperty, animation);
                };
            }
        }

        #endregion

    }
}
