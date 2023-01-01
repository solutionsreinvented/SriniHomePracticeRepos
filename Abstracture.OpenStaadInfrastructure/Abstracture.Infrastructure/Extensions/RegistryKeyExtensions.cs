using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace Abstracture.OpenStaadInfrastructure.Extensions
{
    public static class RegistryKeyExtensions
    {
        public static string FullValueContaining(this RegistryKey keyContainingSearchString, string searchString)
        {
            foreach (var value in from valueName in keyContainingSearchString.GetValueNames()
                                  let value = keyContainingSearchString.GetValue(valueName).ToString()
                                  select value)
            {
                if (!value.Contains(searchString))
                {
                    continue;
                }

                return value;
            }

            return null;
        }

        public static IEnumerable<RegistryKey> AddKeysRecursivelyTo(this RegistryKey registryKey, List<RegistryKey> collectedKeys)
        {
            var subKeys = registryKey.GetSubKeyNames().Select(s => registryKey.OpenSubKey(s));

            foreach (var key in subKeys)
            {
                collectedKeys.Add(key);
                key.AddKeysRecursivelyTo(collectedKeys);
            }

            return collectedKeys;
        }

    }
}
