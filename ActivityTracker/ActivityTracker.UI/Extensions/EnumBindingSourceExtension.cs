using System;
using System.Windows.Markup;

namespace ActivityTracker.UI.Extensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
            {
                throw new Exception($"{nameof(EnumType)} must not be null and of type Enum");
            }

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }

        public Type EnumType { get; private set; }
    }
}
