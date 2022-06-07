using System;

namespace AllCorePracticeApp.Attributes
{
    internal class RegularExpressionAttribute : Attribute
    {
        public string Pattern { get; set; }

        public RegularExpressionAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
