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
        private readonly IPropertyProcessorProvider processorProvider;

        public RouteValueTransformer(IPropertyProcessorProvider processorProvider)
        {
            this.processorProvider = processorProvider;
        }

        public virtual RouteValueDictionary Transform(object request)
        {
            var requestType = request.GetType();
            var processor = processorProvider.ProviderForRequestType(requestType);
            var properties = requestType.GetProperties();
            var result = new RouteValueDictionary();

            foreach (var requestProperty in properties)
            {
                if (processor.ShouldIgnoreRequestProperty(requestProperty))
                {
                    continue;
                }

                var propertyValue = requestProperty.GetValue(request);

                if (propertyValue is ISearch search)
                {
                    var searchType = search.GetType();
                    var searchProperties = searchType.GetProperties();
                    foreach (var searchProperty in searchProperties)
                    {
                        // Ignore properties that are never bound or bound from route
                        if (processor.ShouldIgnoreRequestProperty(searchProperty))
                        {
                            continue;
                        }

                        // Ignore properties which don't have a value, although all AbstractSearch inheritors should be initialized in the Query
                        var searchPropertyValue = searchProperty.GetValue(search);
                        if (searchPropertyValue == null)
                        {
                            continue;
                        }

                        // Ignore properties which have default value
                        if (processor.IsDefaultRequestSearchPropertyValue(requestProperty, searchPropertyValue))
                        {
                            continue;
                        }

                        AddPropertyValues($"{requestProperty.Name}.{searchProperty.Name}", searchPropertyValue, searchProperty, result);
                    }

                    continue;
                }

                // Ignore properties which have default value
                if (processor.IsDefaultRequestPropertyValue(requestProperty, propertyValue))
                {
                    continue;
                }
                

                AddPropertyValues($"{requestProperty.Name}", propertyValue, requestProperty, result);
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