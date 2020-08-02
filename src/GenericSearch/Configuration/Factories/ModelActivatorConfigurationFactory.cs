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
            if (source.RequestFactoryDefinition == null)
            {
                return defaultConfiguration;
            }

            var definition = source.RequestFactoryDefinition;

            if (definition.FactoryMethod != null)
            {
                return new ModelActivatorConfiguration(_ => definition.FactoryMethod());
            }

            if (definition.FactoryType != null)
            {
                return new ModelActivatorConfiguration(definition.FactoryType);
            }

            if (definition.FactoryServiceProvider != null)
            {
                return new ModelActivatorConfiguration((sp, _) => definition.FactoryServiceProvider(sp));
            }

            return defaultConfiguration;
        }
    }
}