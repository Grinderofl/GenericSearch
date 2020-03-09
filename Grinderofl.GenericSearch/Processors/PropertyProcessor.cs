#pragma warning disable 1591
using System;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;
using Grinderofl.GenericSearch.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.Processors
{
    public class PropertyProcessor : IPropertyProcessor
    {
        private readonly ISearchConfiguration configuration;

        public PropertyProcessor(ISearchConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool IsDefaultRequestPropertyValue(PropertyInfo searchPropertyInfo, object value)
        {
            var defaultValue = searchPropertyInfo.GetCustomAttribute<DefaultValueAttribute>();
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

            if (searchPropertyInfo.PropertyType == typeof(TrueBooleanSearch) || searchPropertyInfo.PropertyType == typeof(BooleanSearch))
            {
                return value.Equals(false);
            }

            if (searchPropertyInfo.PropertyType == typeof(OptionalBooleanSearch))
            {
                return value.Equals(null);
            }

            return false;
        }

        public virtual bool ShouldIgnoreRequestProperty(PropertyInfo requestProperty)
        {
            return requestProperty.HasAttribute<BindNeverAttribute>();
        }

        public virtual bool ShouldIgnoreEntityProperty(PropertyInfo entityProperty)
        {
            return !entityProperty.HasAttribute<DisplayAttribute>();
        }

        public virtual bool IsDefaultSearchPropertyValue(PropertyInfo propertyInfo, object value)
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

            // Check if property is a pagination property
            if (configuration.PageExpression != null && configuration.PageExpression != NullPageExpression.Instance)
            {
                if (propertyInfo.Name == configuration.PageExpression.RequestPageProperty.Name)
                {
                    return configuration.PageExpression.DefaultPage.Equals(value);
                }

                if (propertyInfo.Name == configuration.PageExpression.RequestRowsProperty.Name)
                {
                    return configuration.PageExpression.DefaultRows.Equals(value);
                }
            }

            // Check if it's a sort expression
            if (configuration.SortExpression != null && configuration.SortExpression != NullSortExpression.Instance)
            {
                if (propertyInfo.Name == configuration.SortExpression.RequestSortByProperty.Name)
                {
                    if (configuration.SortExpression.DefaultSortBy == null)
                    {
                        return value == null;
                    }
                    return configuration.SortExpression.DefaultSortBy.Name.ToLowerInvariant().Equals(value);
                }

                if (propertyInfo.Name == configuration.SortExpression.RequestSortDirectionProperty.Name)
                {
                    return configuration.SortExpression.DefaultSortDirection.Equals(value);
                }
            }

            return false;
        }
    }
}