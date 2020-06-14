using System;
using GenericSearch.Definition;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
{
    public class RequestFactoryConfigurationFactory : IRequestFactoryConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public RequestFactoryConfigurationFactory(IOptions<GenericSearchOptions> options) => this.options = options.Value;

        public RequestFactoryConfiguration Create(IListDefinition source)
        {
            if (source.RequestFactoryDefinition == null)
            {
                return new RequestFactoryConfiguration()
                {
                    FactoryServiceProvider = options.DefaultRequestFactoryServiceProvider,
                    FactoryType = options.DefaultRequestFactoryType,
                    FactoryMethod = options.DefaultRequestFactoryMethod
                };
            }

            var definition = source.RequestFactoryDefinition;

            if (definition.FactoryMethod != null)
            {
                return new RequestFactoryConfiguration(_ => definition.FactoryMethod());
            }

            if (definition.FactoryType != null)
            {
                return new RequestFactoryConfiguration(definition.FactoryType);
            }

            if (definition.FactoryServiceProvider != null)
            {
                return new RequestFactoryConfiguration((sp, _) => definition.FactoryServiceProvider(sp));
            }

            return new RequestFactoryConfiguration()
            {
                FactoryServiceProvider = options.DefaultRequestFactoryServiceProvider,
                FactoryType = options.DefaultRequestFactoryType,
                FactoryMethod = options.DefaultRequestFactoryMethod
            };
        }
    }
}