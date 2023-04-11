using System.Collections.Generic;
using System.Runtime.CompilerServices;

using ActivityTracker.Domain.Base;

namespace ActivityTracker.Domain.Stores
{
    public class PropertyStore : NotifyPropertyChanged
    {
        readonly Dictionary<string, object> _propertiesDictionary = new();

        protected T Get<T>(T defaultValueOfSpecifiedType = default, [CallerMemberName] string propertyName = "")
        {
            lock (_propertiesDictionary)
            {
                if (_propertiesDictionary.TryGetValue(propertyName, out var retrievedValue) && retrievedValue is T valueOfSpecifiedType)
                {
                    return valueOfSpecifiedType;
                }
            }

            return defaultValueOfSpecifiedType;
        }

        protected void Set<T>(T value, [CallerMemberName] string propertyName = "")
        {
            lock (_propertiesDictionary)
            {
                if (_propertiesDictionary.ContainsKey(propertyName))
                {
                    _propertiesDictionary[propertyName] = value;
                }
                else
                {
                    _propertiesDictionary.Add(propertyName, value);
                }

                RaiseMultiplePropertiesChanged(propertyName);
            }
        }
    }
}
