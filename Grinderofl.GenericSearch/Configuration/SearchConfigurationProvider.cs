#pragma warning disable 1591
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grinderofl.GenericSearch.Configuration
{
    public class SearchConfigurationProvider : ISearchConfigurationProvider
    {
        private readonly IEnumerable<ISearchConfiguration> configurations;
        private readonly Dictionary<Type, ISearchConfiguration> entityTypeRegistry = new Dictionary<Type, ISearchConfiguration>();
        private readonly Dictionary<Type, ISearchConfiguration> requestTypeRegistry = new Dictionary<Type, ISearchConfiguration>();
        private readonly Dictionary<Type, ISearchConfiguration> resultTypeRegistry = new Dictionary<Type, ISearchConfiguration>();

        public SearchConfigurationProvider(IEnumerable<ISearchConfiguration> configurations, IOptions<GenericSearchOptions> options)
        {
            this.configurations = InitialiseConfigurations(configurations, options.Value);
            InitialiseRegistries();
        }

        private IEnumerable<ISearchConfiguration> InitialiseConfigurations(IEnumerable<ISearchConfiguration> originalConfigurations, GenericSearchOptions options)
        {
            return options.ConventionOptions.UseConventions
                       ? originalConfigurations.Select(x => new ConventionSearchConfiguration((SearchConfigurationBase)x, options)).ToArray()
                       : originalConfigurations;
        }

        private void InitialiseRegistries()
        {
            foreach (var configuration in configurations)
            {
                entityTypeRegistry.Add(configuration.EntityType, configuration);
                requestTypeRegistry.Add(configuration.RequestType, configuration);
                resultTypeRegistry.Add(configuration.ResultType, configuration);
            }
        }

        public virtual ISearchConfiguration ForEntityType(Type entityType)
        {
            return entityTypeRegistry.TryGetValue(entityType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForRequestType(Type requestType)
        {
            return requestTypeRegistry.TryGetValue(requestType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForResultType(Type resultType)
        {
            return resultTypeRegistry.TryGetValue(resultType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForEntityAndRequestType(Type entityType, Type requestType)
        {
            if (requestTypeRegistry.TryGetValue(requestType, out var configuration) && configuration.EntityType == entityType)
            {
                return configuration;
            }

            return configurations.FirstOrDefault(x => x.EntityType == entityType && x.RequestType == requestType);
        }

        public ISearchConfiguration ForRequestParametersAndResultType(IEnumerable<ParameterDescriptor> actionDescriptorParameters, Type resultType)
        {
            return configurations.FirstOrDefault(x => x.ResultType == resultType && actionDescriptorParameters.Any(ad => ad.ParameterType == x.RequestType));
        }

        public ISearchConfiguration ForRequestParametersType(IEnumerable<ParameterDescriptor> actionDescriptorParameters)
        {
            return configurations.FirstOrDefault(x => actionDescriptorParameters.Any(ad => ad.ParameterType == x.RequestType));
        }
    }
}