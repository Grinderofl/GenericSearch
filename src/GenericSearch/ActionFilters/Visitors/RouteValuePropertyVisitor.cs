using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters.Visitors
{
    /// <summary>
    /// Provides a property visitor to populate an instance of <see cref="routeValueDictionary"/>
    /// </summary>
    public abstract class RouteValuePropertyVisitor : AbstractPropertyVisitor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RouteValuePropertyVisitor"/>
        /// </summary>
        /// <param name="routeValueDictionary">RouteValueDictionary to populate</param>
        protected RouteValuePropertyVisitor(RouteValueDictionary routeValueDictionary)
        {
            this.routeValueDictionary = routeValueDictionary;
        }

        private readonly RouteValueDictionary routeValueDictionary;

        /// <summary>
        /// Checks whether the provided <paramref name="value"/> has a default value as specified by the <paramref name="propertyInfo"/>
        /// </summary>
        /// <param name="propertyInfo">Property Info to check default value from</param>
        /// <param name="value">Value to check</param>
        /// <returns>Result of default value comparison</returns>
        protected virtual bool IsDefaultPropertyValue(PropertyInfo propertyInfo, object value)
        {
            var defaultValue = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValue != null)
            {
                if (defaultValue.Value == null && value == null)
                {
                    return true;
                }

                if (defaultValue.Value == null && value != null)
                {
                    return false;
                }

                return defaultValue.Value.Equals(value);
            }

            return false;
        }

        /// <summary>
        /// Populates the <see cref="RouteValueDictionary"/> with specified <paramref name="key"/> and <paramref name="value"/>,
        /// taking into consideration whether the <paramref name="propertyInfo"/> is Array or Enum and deals with them appropriately.
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="propertyInfo">Info of the property to check</param>
        /// <param name="value">Dictionary value</param>
        protected void PopulateRouteValues(string key, PropertyInfo propertyInfo, object value)
        {
            key = key.ToLowerInvariant();

            if (propertyInfo.PropertyType.IsArray)
            {
                var values = Array.ConvertAll((object[]) value, x => Convert.ToString(x).ToLowerInvariant());
                routeValueDictionary.Add(key, values);
                return;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                routeValueDictionary.Add(key, (int)value);
                return;
            }

            routeValueDictionary.Add(key, value);
        }
    }
}