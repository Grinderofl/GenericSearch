using System;
using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Internal.Activation
{
    public class ModelActivator : IModelActivator
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ModelActivator(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Attempts to create an instance of <see cref="IListConfiguration.RequestType"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="NullReferenceException">
        /// An instance of <see cref="IModelFactory"/> could not be created by <see cref="ActivatorUtilities.GetServiceOrCreateInstance"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// No model factories or factory methods have been defined in <see cref="IListConfiguration.ModelActivatorConfiguration"/>.
        /// </exception>
        /// <returns></returns>
        public object CreateInstance(IListConfiguration source)
        {
            var configuration = source.ModelActivatorConfiguration;

            if (configuration.Method != null)
            {
                return configuration.Method(source.RequestType);
            }

            // TODO: Add some sort of null check for httpContextAccessor?

            if (configuration.Factory != null)
            {
                return configuration.Factory(httpContextAccessor.HttpContext.RequestServices, source.RequestType);
            }

            if (configuration.FactoryType != null)
            {
                var factory = ActivatorUtilities.GetServiceOrCreateInstance(httpContextAccessor.HttpContext.RequestServices, configuration.FactoryType) as IModelFactory;
                if (factory == null)
                {
                    throw new NullReferenceException($"Unable to create an instance of '{configuration.FactoryType.FullName}'.");
                }

                return factory.Create(source.RequestType);
            }

            throw new ArgumentException($"No suitable factory methods defined", nameof(source.ModelActivatorConfiguration));
        }
    }
}