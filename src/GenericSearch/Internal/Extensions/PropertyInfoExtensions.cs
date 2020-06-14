using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GenericSearch.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class PropertyInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
            => propertyInfo.GetCustomAttribute<TAttribute>() != null;

        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? propertyInfo.Name;
        }

        public static T GetValue<T>(this PropertyInfo propertyInfo, object source)
        {
            return (T) propertyInfo.GetValue(source);
        }

        public static T GetDefaultValue<T>(this PropertyInfo propertyInfo)
        {
            var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute == null)
            {
                return default;
            }

            return (T) defaultValueAttribute.Value;
        }
    }
}