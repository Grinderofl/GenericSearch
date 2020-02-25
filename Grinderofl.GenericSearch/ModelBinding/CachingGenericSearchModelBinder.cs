#pragma warning disable 1591
using Grinderofl.GenericSearch.Caching;
using Grinderofl.GenericSearch.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class CachingGenericSearchModelBinder : GenericSearchModelBinder
    {
        private readonly IRequestModelCacheProvider cacheProvider;

        public CachingGenericSearchModelBinder(IRequestBinder requestBinder, ISearchConfiguration configuration, IModelBinder fallbackModelBinder, IRequestModelCacheProvider cacheProvider)
            : base(requestBinder, configuration, fallbackModelBinder)
        {
            this.cacheProvider = cacheProvider;
        }

        public override async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await base.BindModelAsync(bindingContext);
            var cache = cacheProvider.Provide();
            cache.Put(bindingContext.Model);
        }
    }
}