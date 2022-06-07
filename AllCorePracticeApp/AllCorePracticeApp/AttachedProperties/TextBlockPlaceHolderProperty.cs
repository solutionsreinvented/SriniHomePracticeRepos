using System.Windows;

namespace AllCorePracticeApp.AttachedProperties
{
    public class TextBlockPlaceHolderProperty : AttachedPropertyBase<TextBlockPlaceHolderProperty, string>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            base.OnValueChanged(d, e);
        }
    }
}
