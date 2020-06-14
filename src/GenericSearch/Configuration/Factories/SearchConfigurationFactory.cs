using System;
using System.Collections.Generic;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using GenericSearch.Searches.Activation;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Configuration.Factories
{
    public class SearchConfigurationFactory : ISearchConfigurationFactory
    {
        public SearchConfiguration Create(PropertyInfo requestProperty, IListDefinition source)
        {
            var filter = source.SearchDefinitions.GetValueOrDefault(requestProperty);
            var itemProperty = filter?.ItemProperty ??
                               source.ItemType.GetProperty(requestProperty.Name);
            var resultProperty = filter?.ResultProperty ??
                                 source.ResultType.GetProperty(requestProperty.Name);
            var ignored = filter?.Ignored ?? false;
            var factory = filter?.Constructor;
            var activator = filter?.ActivatorType != null
                ? sp => (ISearchActivator) sp.GetRequiredService(filter.ActivatorType)
                : filter?.Activator;

            return new SearchConfiguration(requestProperty)
            {
                ResultProperty = resultProperty,
                ItemProperty = itemProperty,
                Ignored = ignored,
                Constructor = factory,
                Activator = activator
            };
        }
    }
}