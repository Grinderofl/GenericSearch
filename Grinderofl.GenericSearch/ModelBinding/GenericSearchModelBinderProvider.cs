#pragma warning disable 1591
using Grinderofl.GenericSearch.Caching;
using Grinderofl.GenericSearch.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class GenericSearchModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider fallbackModelBinderProvider;

        public GenericSearchModelBinderProvider(IModelBinderProvider fallbackModelBinderProvider)
        {
            this.fallbackModelBinderProvider = fallbackModelBinderProvider;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var configurations = context.Services.GetRequiredService<ISearchConfigurationProvider>();
            var modelTypeConfiguration = configurations.ForRequestType(context.Metadata.ModelType);

            if (modelTypeConfiguration == null)
            {
                return null;
            }

            var modelBinder = fallbackModelBinderProvider.GetBinder(context);

            var options = context.Services.GetRequiredService<IOptions<GenericSearchOptions>>();
            if (options.Value.CacheRequestModel)
            {
                var cacheProvider = context.Services.GetRequiredService<IRequestModelCacheProvider>();
                return new CachingGenericSearchModelBinder(modelTypeConfiguration, modelBinder, cacheProvider);
            }

            return new GenericSearchModelBinder(modelTypeConfiguration, modelBinder);
        }


    }


}