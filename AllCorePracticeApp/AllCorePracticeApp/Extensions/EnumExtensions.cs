using AllCorePracticeApp.Attributes;
using System;
using System.Reflection;

namespace AllCorePracticeApp.Extensions
{
    public static class EnumExtensions
    {
        public static string GetRegularExpression<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException("{0} must be of Enum type", Enum.GetName(typeof(T), enumerationValue));
            }

            MemberInfo[] member = type.GetMember(enumerationValue.ToString());

            if (member.Length != 0)
            {
                object[] customAttributes = member[0].GetCustomAttributes(typeof(RegularExpressionAttribute), inherit: false);
                if (customAttributes.Length != 0)
                {
                    return ((RegularExpressionAttribute)customAttributes[0]).Pattern;
                }
            }

            return enumerationValue.ToString();
        }
    }
}
