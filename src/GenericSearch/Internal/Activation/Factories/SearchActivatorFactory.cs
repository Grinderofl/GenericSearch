using System;
using System.Collections.Generic;
using GenericSearch.Searches.Activation;

namespace GenericSearch.Internal.Activation.Factories
{
    public class SearchActivatorFactory : ISearchActivatorFactory
    {
        private readonly Dictionary<Type, ISearchActivator> cache = new Dictionary<Type, ISearchActivator>();
        private readonly IServiceProvider serviceProvider;
        public SearchActivatorFactory(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;


        public ISearchActivator Create(Type searchType)
        {
            // Since the SearchActivatorFactory is created with a scoped lifespan by the
            // service provider, we can cache the search activator instances for the remainder
            // of the request and prevent unnecessary initialisations.

            if (cache.ContainsKey(searchType))
            {
                return cache[searchType];
            }

            var activatorType = typeof(ISearchActivator<>).MakeGenericType(searchType);
            var searchActivator = serviceProvider.GetService(activatorType) as ISearchActivator ??
                                  new FallbackSearchActivator(searchType);

            cache[searchType] = searchActivator;
            return searchActivator;
        }
    }
}