using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Abstracture.OpenStaadInfrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static RegistryKey KeyContainingStringInItsValuePrev(this IEnumerable<RegistryKey> keysWithValues, string searchString)
        {
            foreach (var key in keysWithValues)
            {
                foreach (var valueName in key.GetValueNames())
                {
                    if (!key.GetValue(valueName).ToString().Contains(searchString))
                    {
                        continue;
                    }
                    return key;
                }
            }

            return null;
        }

        public static RegistryKey KeyContainingStringInItsValue(this IEnumerable<RegistryKey> keysWithValues, string searchString)
        {
            return keysWithValues.Where(k => k.GetValueNames().Any(n => k.GetValue(n).ToString().Contains(searchString))).FirstOrDefault();
        }

    }
}
