using System;
using GenericSearch.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.ModelBinders.Activation
{
    public class ModelActivator : IModelActivator
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ModelActivator(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public object Activate(IListConfiguration source)
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