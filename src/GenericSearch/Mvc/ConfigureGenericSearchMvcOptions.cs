using System.Linq;
using GenericSearch.ActionFilters;
using GenericSearch.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;

namespace GenericSearch.Mvc
{
    /// <summary>
    /// Configures MVC options with GenericSearch additions
    /// </summary>
    public class ConfigureGenericSearchMvcOptions : IConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Configures MVC options
        /// </summary>
        /// <param name="options"><see cref="MvcOptions"/></param>
        public void Configure(MvcOptions options)
        {
            var fallbackModelBinderProvider = options.ModelBinderProviders.First(x => x is ComplexTypeModelBinderProvider);
            options.ModelBinderProviders.Insert(0, new GenericSearchModelBinderProvider(fallbackModelBinderProvider));
            options.Filters.Add<RedirectPostToGetAsyncActionFilter>();
            options.Filters.Add<TransferRequestFilterPropertiesAsyncActionFilter>();
        }
    }
}