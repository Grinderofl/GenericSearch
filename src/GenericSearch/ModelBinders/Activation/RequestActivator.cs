using System;
using GenericSearch.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.ModelBinders.Activation
{
    public class RequestActivator : IRequestActivator
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestActivator(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public object Activate(ListConfiguration source)
        {
            var configuration = source.RequestFactoryConfiguration;

            if (configuration.FactoryMethod != null)
            {
                return configuration.FactoryMethod(source.RequestType);
            }

            if (configuration.FactoryServiceProvider != null)
            {
                return configuration.FactoryServiceProvider(httpContextAccessor.HttpContext.RequestServices, source.RequestType);
            }

            if (configuration.FactoryType != null)
            {
                var factory = ActivatorUtilities.GetServiceOrCreateInstance(httpContextAccessor.HttpContext.RequestServices, configuration.FactoryType) as IRequestFactory;
                return factory?.Create(source.RequestType);
            }

            throw new ArgumentException($"No suitable factory methods defined", nameof(source.RequestFactoryConfiguration));
        }
    }
}