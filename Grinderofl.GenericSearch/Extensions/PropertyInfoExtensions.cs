using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Grinderofl.GenericSearch.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
            => propertyInfo.GetCustomAttribute<TAttribute>() != null;

        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? propertyInfo.Name;
        }
    }
}