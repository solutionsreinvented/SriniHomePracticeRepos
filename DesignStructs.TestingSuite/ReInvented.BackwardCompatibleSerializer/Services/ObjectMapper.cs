using Newtonsoft.Json.Linq;

using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.Domain.Tass.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReInvented.BackwardCompatibleSerializer.Services
{
    public static class ObjectMapper
    {
        private static readonly Dictionary<Type, Type> InterfaceToConcreteMapping = new Dictionary<Type, Type>()
        {
            { typeof(IFloor), typeof(Floor) }
        };

        private static readonly Dictionary<Type, object> ExistingObjects = new Dictionary<Type, object>();

        public static object MapObject(Type targetType, JObject source)
        {
            // Resolve the concrete type if $type is present
            if (source.TryGetValue("$type", out JToken typeToken))
            {
                string typeName = typeToken.ToString();
                targetType = Type.GetType(typeName) ?? targetType;
            }
            else if (targetType.IsInterface || targetType.IsAbstract)
            {
                targetType = GetConcreteTypeForInterface(targetType)
                             ?? throw new InvalidOperationException($"Cannot instantiate an interface or abstract type: {targetType}");
            }

            // Instantiate the object
            object targetObject = CreateObjectWithConstructor(targetType, source);

            // Map properties from source to the target object
            foreach (var property in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite) continue;

                JToken token = source[property.Name];
                if (token != null && token.Type != JTokenType.Null)
                {
                    object value = MapPropertyValue(property.PropertyType, token);
                    property.SetValue(targetObject, value);
                }
            }

            return targetObject;
        }

        private static Type GetConcreteTypeForInterface(Type interfaceType)
        {
            return InterfaceToConcreteMapping.TryGetValue(interfaceType, out var concreteType) ? concreteType : null;
        }

        private static object MapPropertyValue(Type propertyType, JToken token)
        {
            if (IsSimpleType(propertyType))
            {
                // Handle simple types
                return token.ToObject(propertyType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
            {
                // Handle collections
                var itemType = propertyType.IsArray
                    ? propertyType.GetElementType()
                    : propertyType.GenericTypeArguments.FirstOrDefault();

                if (itemType != null && token is JArray jArray)
                {
                    IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                    foreach (var item in jArray)
                    {
                        list.Add(MapObject(itemType, item as JObject));
                    }

                    return propertyType.IsArray ? list.Cast<object>().ToArray() : list;
                }
            }
            else if (token is JObject nestedObject)
            {
                // Handle nested objects
                return MapObject(propertyType, nestedObject);
            }

            return null;
        }

        private static object CreateObjectWithConstructor(Type targetType, JObject source)
        {
            // Check for existing object in the cache
            if (ExistingObjects.TryGetValue(targetType, out object existingObject))
                return existingObject;

            // Find the most suitable constructor
            var constructor = targetType.GetConstructors()
                                        .OrderByDescending(c => c.GetParameters().Length)
                                        .FirstOrDefault();

            if (constructor == null)
                throw new InvalidOperationException($"No suitable constructor found for type {targetType}");

            // Resolve constructor parameters
            var parameters = constructor.GetParameters()
                                         .Select(p => ResolveConstructorParameter(p.ParameterType, source))
                                         .ToArray();

            // Create the object using the resolved constructor parameters
            var targetObject = constructor.Invoke(parameters);

            // Cache the created object
            ExistingObjects[targetType] = targetObject;

            return targetObject;
        }

        private static object ResolveConstructorParameter(Type parameterType, JObject source)
        {
            // Handle interface types by mapping to concrete types
            Type concreteType = GetConcreteTypeForInterface(parameterType);
            if (concreteType != null)
                parameterType = concreteType;

            // Check for an existing object in the cache
            if (ExistingObjects.TryGetValue(parameterType, out var existingObject))
                return existingObject;

            // If the source contains an object matching the parameter type, use it
            if (source.TryGetValue(parameterType.Name, StringComparison.OrdinalIgnoreCase, out JToken matchingToken) && matchingToken is JObject nestedSource)
            {
                return MapObject(parameterType, nestedSource);
            }

            // Recursively create the parameter if nothing exists
            return CreateObjectWithConstructor(parameterType, new JObject());
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(double)
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(DateTime)
                   || type == typeof(Guid)
                   || type == typeof(TimeSpan);
        }
    }

}