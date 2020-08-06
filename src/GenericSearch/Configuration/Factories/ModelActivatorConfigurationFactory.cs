using System;
using GenericSearch.Definition;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
{
    public class ModelActivatorConfigurationFactory : IModelActivatorConfigurationFactory
    {
        private readonly GenericSearchOptions options;
        private readonly IModelActivatorConfiguration defaultConfiguration;

        public ModelActivatorConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
            defaultConfiguration = CreateDefaultConfiguration();
        }

        private IModelActivatorConfiguration CreateDefaultConfiguration() => 
            new ModelActivatorConfiguration(options.DefaultModelActivatorType, 
                                            options.DefaultModelActivatorMethod, 
                                            options.DefaultModelActivatorResolver);

        public IModelActivatorConfiguration Create(IListDefinition source)
        {
            if (source.ModelActivatorDefinition == null)
            {
                return defaultConfiguration;
            }

            var definition = source.ModelActivatorDefinition;

            if (definition.Method != null)
            {
                return new ModelActivatorConfiguration(definition.Method);
            }

            if (definition.FactoryType != null)
            {
                return new ModelActivatorConfiguration(definition.FactoryType);
            }

            if (definition.Factory != null)
            {
                return new ModelActivatorConfiguration(definition.Factory);
            }

            return defaultConfiguration;
        }
    }
}