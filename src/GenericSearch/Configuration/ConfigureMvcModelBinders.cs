using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GenericSearch.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ConfigureMvcModelBinders : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            var fallbackBinderProvider = options.ModelBinderProviders.First(x => x is ComplexTypeModelBinderProvider);
            var binderProvider = new GenericSearchModelBinderProvider(fallbackBinderProvider);
            options.ModelBinderProviders.Insert(0, binderProvider);
        }
    }
}