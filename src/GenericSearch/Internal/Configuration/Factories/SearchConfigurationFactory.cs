using System.Collections.Generic;
using System.Reflection;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Internal.Definition;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Internal.Configuration.Factories
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

            var itemPropertyPaths = search?.ItemPropertyPaths ??
                                    new[] {propertyPathFinder.Find(source.ItemType, requestProperty.Name)};

            var factory = search?.Constructor;
            var activator = search?.ActivatorType != null
                ? sp => (ISearchActivator) sp.GetRequiredService(search.ActivatorType)
                : search?.Activator;

            return new SearchConfiguration(requestProperty)
            {
                ResultProperty = resultProperty,
                ItemPropertyPath = itemPropertyPaths,
                Ignored = ignored,
                Constructor = factory,
                Activator = activator
            };
        }
    }
}