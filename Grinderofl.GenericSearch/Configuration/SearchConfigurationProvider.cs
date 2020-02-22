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
        private readonly Dictionary<Type, ISearchConfiguration> entityTypeLookupRegistry = new Dictionary<Type, ISearchConfiguration>();
        private readonly Dictionary<Type, ISearchConfiguration> requestTypeLookupRegistry = new Dictionary<Type, ISearchConfiguration>();
        private readonly Dictionary<Type, ISearchConfiguration> resultTypeLookupRegistry = new Dictionary<Type, ISearchConfiguration>();

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
                entityTypeLookupRegistry.Add(configuration.EntityType, configuration);
                requestTypeLookupRegistry.Add(configuration.RequestType, configuration);
                resultTypeLookupRegistry.Add(configuration.ResultType, configuration);
            }
        }

        public virtual ISearchConfiguration ForEntityType(Type entityType)
        {
            return entityTypeLookupRegistry.TryGetValue(entityType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForRequestType(Type requestType)
        {
            return requestTypeLookupRegistry.TryGetValue(requestType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForResultType(Type resultType)
        {
            return resultTypeLookupRegistry.TryGetValue(resultType, out var configuration)
                       ? configuration
                       : null;
        }

        public virtual ISearchConfiguration ForEntityAndRequestType(Type entityType, Type requestType)
        {
            if (requestTypeLookupRegistry.TryGetValue(requestType, out var configuration) && configuration.EntityType == entityType)
            {
                return configuration;
            }

            return configurations.FirstOrDefault(x => x.EntityType == entityType && x.RequestType == requestType);
        }

        public ISearchConfiguration ForRequestAndResultType(Type requestType, Type resultType)
        {
            return configurations.FirstOrDefault(x => x.RequestType == requestType && x.ResultType == resultType);
        }

        public ISearchConfiguration ForRequestParametersAndResultType(IEnumerable<ParameterDescriptor> actionDescriptorParameters, Type resultType)
        {
            return configurations.FirstOrDefault(x => x.ResultType == resultType && actionDescriptorParameters.Any(ad => ad.ParameterType == x.RequestType));
        }

        public ISearchConfiguration ForRequestParametersType(IEnumerable<ParameterDescriptor> actionDescriptorParameters)
        {
            return configurations.FirstOrDefault(x => actionDescriptorParameters.Any(ad => ad.ParameterType == x.RequestType));
        }

        public bool HasEntityType(Type entityType)
        {
            return entityTypeLookupRegistry.ContainsKey(entityType);
        }

        public bool HasRequestType(Type requestType)
        {
            return requestTypeLookupRegistry.ContainsKey(requestType);
        }

        public bool HasResultType(Type resultType)
        {
            return resultTypeLookupRegistry.ContainsKey(resultType);
        }
    }
}