using System;
using System.Collections.Generic;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using GenericSearch.Searches.Activation;
using GenericSearch.Searches.Activation.Finders;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Configuration.Factories
{
    public class SearchConfigurationFactory : ISearchConfigurationFactory
    {
        private readonly IPropertyPathFinder propertyPathFinder;

        public SearchConfigurationFactory(IPropertyPathFinder propertyPathFinder) => this.propertyPathFinder = propertyPathFinder;

        public SearchConfiguration Create(PropertyInfo requestProperty, IListDefinition source)
        {
            var search = source.SearchDefinitions.GetValueOrDefault(requestProperty);
            var resultProperty = search?.ResultProperty ??
                                 source.ResultType.GetProperty(requestProperty.Name);
            var ignored = search?.Ignored ?? false;

            var entityPath = ignored
                ? null
                : search?.ItemPropertyPath ??
                  propertyPathFinder.Find(source.ItemType, requestProperty.Name);

            var factory = search?.Constructor;
            var activator = search?.ActivatorType != null
                ? sp => (ISearchActivator) sp.GetRequiredService(search.ActivatorType)
                : search?.Activator;

            return new SearchConfiguration(requestProperty)
            {
                ResultProperty = resultProperty,
                ItemPropertyPath = entityPath,
                Ignored = ignored,
                Constructor = factory,
                Activator = activator
            };
        }
    }
}