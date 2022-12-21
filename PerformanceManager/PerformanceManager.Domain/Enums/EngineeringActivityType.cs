﻿using PerformanceManager.Domain.Converters;
using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EngineeringActivityType
    {
        Design,
        Drafting
    }
}
