using Grinderofl.GenericSearch.Configuration.Internal.Caching;
using Grinderofl.GenericSearch.Internal;
using Grinderofl.GenericSearch.Providers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Grinderofl.GenericSearch.ModelBinding
{
    /// <summary>
    /// Provides the default implementation of creating model binders for request/parameter types for Generic Search types
    /// </summary>
    public class GenericSearchModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider fallbackModelBinderProvider;

        /// <summary>
        /// Initializes a new instance of default Generic Search model binder provider
        /// </summary>
        /// <param name="fallbackModelBinderProvider"></param>
        public GenericSearchModelBinderProvider(IModelBinderProvider fallbackModelBinderProvider)
        {
            this.fallbackModelBinderProvider = fallbackModelBinderProvider;
        }

        /// <summary>
        /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" /> based on <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />.</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var configurationProvider = context.Services.GetRequiredService<IFilterConfigurationProvider>();
            var configuration = configurationProvider.Provide(context.Metadata.ModelType);

            if (configuration == null)
            {
                return null;
            }


            var modelCacheProvider = context.Services.GetRequiredService<IModelCacheProvider>();
            
            var fallbackModelBinder = fallbackModelBinderProvider.GetBinder(context);
            return new GenericSearchModelBinder(configuration, fallbackModelBinder, modelCacheProvider);
        }
    }

}