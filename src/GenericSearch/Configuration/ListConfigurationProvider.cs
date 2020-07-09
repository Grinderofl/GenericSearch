using System;
using System.Collections.Generic;
using System.Linq;
using GenericSearch.Configuration.Factories;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
{
    public class ListConfigurationProvider : IListConfigurationProvider
    {
        internal readonly Dictionary<Type, IListConfiguration> Configurations = new Dictionary<Type, IListConfiguration>();
        private readonly GenericSearchOptions options;

        public ListConfigurationProvider(IEnumerable<IListDefinitionSource> listDefinitions, IListConfigurationFactory listConfigurationFactory, IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
            InitializeConfigurations(listDefinitions, listConfigurationFactory);
        }

        private void InitializeConfigurations(IEnumerable<IListDefinitionSource> listDefinitions, IListConfigurationFactory listConfigurationFactory)
        {
            foreach (var configurationSource in options.Definitions.Union(listDefinitions.SelectMany(x => x.Definitions)))
            {
                if (Configurations.ContainsKey(configurationSource.RequestType))
                {
                    throw new InvalidFilterConfigurationException($"There is already a configuration for Request Type '{configurationSource.RequestType.FullName}'");
                }
                var configuration = listConfigurationFactory.Create(configurationSource);
                Configurations.Add(configuration.RequestType, configuration);
            }
        }

        public IListConfiguration GetConfiguration(Type requestType) =>
            Configurations.ContainsKey(requestType)
                ? Configurations[requestType]
                : null;
    }
}