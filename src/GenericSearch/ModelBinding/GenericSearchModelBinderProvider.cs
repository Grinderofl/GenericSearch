using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.ModelBinding
{
    public class GenericSearchModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider fallbackModelBinderProvider;

        public GenericSearchModelBinderProvider(IModelBinderProvider fallbackModelBinderProvider) => 
            this.fallbackModelBinderProvider = fallbackModelBinderProvider;

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var configurationProvider = context.Services.GetRequiredService<IListConfigurationProvider>();
            var configuration = configurationProvider.GetConfiguration(context.Metadata.ModelType);

            if (configuration == null)
            {
                return null;
            }

            var fallbackBinder = fallbackModelBinderProvider.GetBinder(context);
            var httpContextAccessor = context.Services.GetRequiredService<IHttpContextAccessor>();
            var serviceProvider = httpContextAccessor.HttpContext.RequestServices;

            var modelBinder = ActivatorUtilities.CreateInstance<GenericSearchModelBinder>(serviceProvider, configuration, fallbackBinder);
            return modelBinder;
        }
    }
}