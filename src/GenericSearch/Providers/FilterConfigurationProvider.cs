using System;
using System.Collections.Generic;
using System.Linq;
using GenericSearch.Configuration;
using GenericSearch.Exceptions;

namespace GenericSearch.Providers
{
    /// <summary>
    /// Default implementation of Filter Configuration provisioning for searching and model binding
    /// </summary>
    public class FilterConfigurationProvider : IFilterConfigurationProvider
    {
        private readonly Dictionary<Type, IFilterConfiguration> filterConfigurations = new Dictionary<Type, IFilterConfiguration>();
        
        private readonly IFilterConfigurationFactory filterConfigurationFactory;

        /// <summary>
        /// Initialises a new instance of <see cref="FilterConfigurationProvider"/>
        /// </summary>
        /// <param name="filterConfigurationFactory">Filter Configuration Factory</param>
        /// <param name="filterProfiles">Filter Profiles from service provider</param>
        public FilterConfigurationProvider(IFilterConfigurationFactory filterConfigurationFactory, IEnumerable<IGenericSearchProfile> filterProfiles)
        {
            this.filterConfigurationFactory = filterConfigurationFactory;
            CreateFilterConfigurations(filterProfiles);
        }

        private void CreateFilterConfigurations(IEnumerable<IGenericSearchProfile> profiles)
        {
            var configurations = profiles.SelectMany(x => x.FilterConfigurations)
                                         .Select(filterConfigurationFactory.Create);

            foreach (var configuration in configurations)
            {
                if (filterConfigurations.ContainsKey(configuration.RequestType))
                {
                    throw new InvalidFilterConfigurationException($"More than one Filter Configuration was found for '{configuration.RequestType.FullName}'");
                }

                filterConfigurations.Add(configuration.RequestType, configuration);
            }
        }
        
        /// <summary>
        /// Provides a <see cref="IFilterConfiguration"/> for given request/parameter model type.
        /// </summary>
        /// <param name="modelType">Request/Parameter model type</param>
        /// <returns>Instance of <see cref="IFilterConfiguration"/></returns>
        public IFilterConfiguration Provide(Type modelType)
        {
            if (filterConfigurations.TryGetValue(modelType, out var configuration))
            {
                return configuration;
            }

            return null;
        }
    }
}