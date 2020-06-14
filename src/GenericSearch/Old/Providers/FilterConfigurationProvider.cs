using System;
using System.Collections.Generic;
using System.Linq;
using GenericSearch.Configuration;
using GenericSearch.Configuration.Internal.Caching;
using GenericSearch.Exceptions;

namespace GenericSearch.Providers
{
    /// <summary>
    /// Default implementation of Filter Configuration provisioning for searching and model binding
    /// </summary>
    public class FilterConfigurationProvider : IFilterConfigurationProvider
    {
        private readonly IFilterConfigurationFactory filterConfigurationFactory;
        private readonly FilterConfigurationCache cache;

        /// <summary>
        /// Initialises a new instance of <see cref="FilterConfigurationProvider"/>
        /// </summary>
        /// <param name="filterConfigurationFactory">Filter Configuration Factory</param>
        /// <param name="filterProfiles">Filter Profiles from service provider</param>
        /// <param name="cache"></param>
        public FilterConfigurationProvider(IFilterConfigurationFactory filterConfigurationFactory, IEnumerable<IGenericSearchProfile> filterProfiles, FilterConfigurationCache cache)
        {
            this.filterConfigurationFactory = filterConfigurationFactory;
            this.cache = cache;
            CreateFilterConfigurations(filterProfiles);
        }

        private void CreateFilterConfigurations(IEnumerable<IGenericSearchProfile> profiles)
        {
            var configurations = profiles.SelectMany(x => x.FilterConfigurations)
                                         .Select(filterConfigurationFactory.Create);

            foreach (var configuration in configurations)
            {
                if (cache.ContainsKey(configuration.RequestType))
                {
                    throw new InvalidFilterConfigurationException($"More than one Filter Configuration was found for '{configuration.RequestType.FullName}'");
                }

                cache.Add(configuration.RequestType, configuration);
            }
        }
        
        /// <summary>
        /// Provides a <see cref="IFilterConfiguration"/> for given request/parameter model type.
        /// </summary>
        /// <param name="modelType">Request/Parameter model type</param>
        /// <returns>Instance of <see cref="IFilterConfiguration"/></returns>
        public IFilterConfiguration Provide(Type modelType)
        {
            if (cache.TryGetValue(modelType, out var configuration))
            {
                return configuration;
            }

            return null;
        }
    }
}