using System;
using System.Windows;

using ReIn.TabControlStyling.Enums;

namespace ReIn.TabControlStyling.Models
{
    public class GeometryResourceExtension : DynamicResourceExtension
    {
        public new GeometryResourceKey ResourceKey
        {
            get
            {
                _ = Enum.TryParse(base.ResourceKey.ToString(), out GeometryResourceKey resourceKey);
                return resourceKey;
            }

            set => base.ResourceKey = value.ToString();
        }
    }
}
