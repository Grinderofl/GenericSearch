#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Processors
{
    public class DefaultPropertyProcessorProvider : IPropertyProcessorProvider
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly Dictionary<Type, IPropertyProcessor> entityTypeProcessorCache = new Dictionary<Type, IPropertyProcessor>();
        private readonly Dictionary<Type, IPropertyProcessor> requestTypeProcessorCache = new Dictionary<Type, IPropertyProcessor>();
        private readonly Dictionary<Type, IPropertyProcessor> resultTypeProcessorCache = new Dictionary<Type, IPropertyProcessor>();

        public DefaultPropertyProcessorProvider(ISearchConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public virtual IPropertyProcessor Provide(ISearchConfiguration configuration)
        {
            return new PropertyProcessor(configuration);
        }

        public virtual IPropertyProcessor ProvideForEntityType(Type entityType)
        {
            if (!entityTypeProcessorCache.TryGetValue(entityType, out var processor))
            {
                var configuration = configurationProvider.ForEntityType(entityType);
                processor = new PropertyProcessor(configuration);
                entityTypeProcessorCache.TryAdd(entityType, processor);
            }

            return processor;
        }

        public IPropertyProcessor ProviderForRequestType(Type requestType)
        {
            if (!requestTypeProcessorCache.TryGetValue(requestType, out var processor))
            {
                var configuration = configurationProvider.ForRequestType(requestType);
                processor = new PropertyProcessor(configuration);
                requestTypeProcessorCache.TryAdd(requestType, processor);
            }

            return processor;
        }

        public IPropertyProcessor ProvideForResultType(Type resultType)
        {
            if (!resultTypeProcessorCache.TryGetValue(resultType, out var processor))
            {
                var configuration = configurationProvider.ForResultType(resultType);
                processor = new PropertyProcessor(configuration);
                resultTypeProcessorCache.TryAdd(resultType, processor);
            }

            return processor;
        }
    }
}