#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Processors;
using Grinderofl.GenericSearch.Searches;
using Microsoft.AspNetCore.Routing;
using System;
using System.Reflection;

namespace Grinderofl.GenericSearch.Transformers
{
    public class RouteValueTransformer : IRouteValueTransformer
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly IPropertyProcessorProvider processorProvider;

        public RouteValueTransformer(ISearchConfigurationProvider configurationProvider, IPropertyProcessorProvider processorProvider)
        {
            this.configurationProvider = configurationProvider;
            this.processorProvider = processorProvider;
        }

        public virtual RouteValueDictionary Transform(object request)
        {
            var requestType = request.GetType();
            var processor = processorProvider.ProviderForRequestType(requestType);
            var properties = requestType.GetProperties();
            var result = new RouteValueDictionary();

            foreach (var property in properties)
            {
                if (processor.ShouldIgnoreRequestProperty(property))
                {
                    continue;
                }

                var propertyValue = property.GetValue(request);

                // Ignore properties which have default value
                if (processor.IsDefaultRequestPropertyValue(property, propertyValue))
                {
                    continue;
                }

                // Deal with AbstractSearch first
                if (propertyValue is ISearch search)
                {
                    if (!search.IsActive())
                    {
                        continue;
                    }

                    var abstractSearchType = search.GetType();
                    var abstractSearchProperties = abstractSearchType.GetProperties();
                    foreach (var abstractSearchProperty in abstractSearchProperties)
                    {
                        // Ignore properties that are never bound or bound from route
                        if (processor.ShouldIgnoreRequestProperty(abstractSearchProperty))
                        {
                            continue;
                        }

                        // Ignore properties which don't have a value, although all AbstractSearch inheritors should be initialized in the Query
                        var abstractSearchPropertyValue = abstractSearchProperty.GetValue(search);
                        if (abstractSearchPropertyValue == null)
                        {
                            continue;
                        }

                        // Ignore properties which have default value
                        if (processor.IsDefaultRequestPropertyValue(abstractSearchProperty, abstractSearchPropertyValue))
                        {
                            continue;
                        }

                        AddPropertyValues($"{property.Name}.{abstractSearchProperty.Name}", abstractSearchPropertyValue, abstractSearchProperty, result);
                    }

                    continue;
                }

                AddPropertyValues($"{property.Name}", propertyValue, property, result);
            }

            return result;
        }

        protected virtual void AddPropertyValues(string key, object propertyValue, PropertyInfo property, RouteValueDictionary parameters)
        {
            key = key.ToLowerInvariant();

            // Array types need special treatment
            if (property.PropertyType.IsArray)
            {
                AddArrayPropertyValues(key, propertyValue, parameters);
                return;
            }

            if (property.PropertyType.IsEnum)
            {
                propertyValue = (int)propertyValue;
            }

            // Not an array? Just add the value as string
            parameters.Add(key, propertyValue);
        }

        protected virtual void AddArrayPropertyValues(string key, object propertyValue, RouteValueDictionary parameters)
        {
            // Convert all items of the array to string ...
            var values = Array.ConvertAll((object[])propertyValue, x => Convert.ToString(x).ToLowerInvariant());
            // ... and add them to the parameter collection.
            parameters.Add(key, values);
        }
    }
}