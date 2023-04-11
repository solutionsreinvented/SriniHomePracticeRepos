using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xaml;

namespace ReIn.NavPractice.MarkupExtensions
{
    public class AnimationResourceExtension : DynamicResourceExtension
    {
        public bool IsShared { get; set; } = true;

        public DependencyObject Target { get; set; }

        public string TargetName { get; set; }

        public PropertyPath TargetProperty { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (base.ProvideValue(serviceProvider) is Timeline animation)
            {
                if (IsShared || animation.IsFrozen)
                {
                    animation = animation.Clone();
                }

                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetName(animation, TargetName);
                Storyboard.SetTargetProperty(animation, TargetProperty);

                return animation;
            }
            else
            {
                throw new XamlException("The referenced resource is not animation");
            }
        }
    }
}
