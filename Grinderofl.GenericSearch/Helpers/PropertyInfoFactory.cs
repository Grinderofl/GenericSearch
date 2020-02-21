#pragma warning disable 1591
using Grinderofl.GenericSearch.Extensions;
using System;
using System.Reflection;

namespace Grinderofl.GenericSearch.Helpers
{
    public static class PropertyInfoFactory
    {
        /// <summary>
        /// Creates a PropertyInfo for the provided generic type using the name of the provided property info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static PropertyInfo Create<T>(PropertyInfo propertyInfo)
        {
            return Create<T>(propertyInfo.Name);
        }

        /// <summary>
        /// Creates a PropertyInfo for the provided generic type using the provided property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo Create<T>(string propertyName)
        {
            var propertyExpression = ExpressionFactory.Create<T>(propertyName);
            return propertyExpression.GetPropertyInfo();
        }

        public static PropertyInfo Create(Type type, string propertyName)
        {
            return type.GetProperty(propertyName);
        }
    }
}