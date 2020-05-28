﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Internal.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetSearchProperties(this Type type)
        {
            return type.GetProperties()
                               .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(ISearch)));
        }

        public static bool IsNullableType(this Type propertyType)
        {
            return propertyType.IsGenericType
                   && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsCollectionType(this Type propertyType)
        {
            if (propertyType.IsArray) return true;

            return propertyType.IsGenericType
                   && typeof(IEnumerable<>)
                      .MakeGenericType(propertyType.GetGenericArguments())
                      .IsAssignableFrom(propertyType);
        }

        /// <summary>
        /// Attempts to get <see cref="PropertyInfo"/> with the provided <paramref name="propertyName"/>
        /// if the <paramref name="propertyName"/> is not null or empty.
        /// </summary>
        /// <param name="type">Type to get <see cref="PropertyInfo"/> from</param>
        /// <param name="propertyName">Name of the property or null</param>
        /// <returns><see cref="PropertyInfo"/> or null</returns>
        public static PropertyInfo? GetPropertyInfo(this Type type, string? propertyName)
            => !string.IsNullOrWhiteSpace(propertyName)
                ? type.GetProperty(propertyName)
                : null;
    }
}