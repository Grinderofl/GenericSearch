using System;
using System.Collections.Generic;

namespace GenericSearch.Searches.Activation.Factories
{
    public class SearchActivatorFactory : ISearchActivatorFactory
    {
        private readonly Dictionary<Type, ISearchActivator> cache = new Dictionary<Type, ISearchActivator>();
        private readonly IServiceProvider serviceProvider;
        public SearchActivatorFactory(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;


        public ISearchActivator Create(Type searchType)
        {
            if (cache.ContainsKey(searchType))
            {
                return cache[searchType];
            }

            var activatorType = typeof(ISearchActivator<>).MakeGenericType(searchType);
            var resolved = serviceProvider.GetService(activatorType) as ISearchActivator ??
                           new FallbackSearchActivator(searchType);
            cache[searchType] = resolved;
            return resolved;
        }
    }
}