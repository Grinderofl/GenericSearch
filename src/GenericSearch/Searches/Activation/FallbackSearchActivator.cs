using System;
using System.Linq;
using System.Reflection;
using GenericSearch.Exceptions;

namespace GenericSearch.Searches.Activation
{
    public class FallbackSearchActivator : ISearchActivator
    {
        private readonly Type searchType;

        public FallbackSearchActivator(Type searchType) => this.searchType = searchType;

        public ISearch Create(PropertyInfo itemProperty)
        {
            var constructors = searchType.GetConstructors();
            if (!constructors.Any())
            {
                throw new SearchPropertyActivationException($"No public constructors found on '{searchType.FullName}'");
            }
            
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                
                // TODO: Consider navigation properties
                if (parameters.Length == 1)
                {
                    // new Search("PropertyName")
                    if (parameters.Single().ParameterType == typeof(string))
                    {
                        return Activator.CreateInstance(searchType, itemProperty.Name) as ISearch;
                    }

                    // TODO: new Search(propertyInfo)
                    if (parameters.Single().ParameterType == typeof(PropertyInfo))
                    {
                        return Activator.CreateInstance(searchType, itemProperty) as ISearch;
                    }

                    // TODO: new Search(property: x => x.Property) -> base(property)
                }

                // new Search()
                if (parameters.Length == 0)
                {
                    return Activator.CreateInstance(searchType) as ISearch;
                }

                // TODO: Resolve constructor arguments from ServiceProvider?
            }

            throw new SearchPropertyActivationException($"No suitable constructors found on '{searchType.FullName}'");
        }
    }
}