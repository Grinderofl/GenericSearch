#pragma warning disable 1591
using Grinderofl.GenericSearch.Caching;
using Grinderofl.GenericSearch.Filters;
using Grinderofl.GenericSearch.ModelBinding;
using Grinderofl.GenericSearch.Processors;
using Grinderofl.GenericSearch.Transformers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration
{
    public class GenericSearchBuilder
    {
        public GenericSearchBuilder(IServiceCollection services, Assembly[] assemblies)
        {
            Services = services;
            Assemblies = assemblies;
            AddGenericSearchServices();
            AddMvcServices();
            AddConfigurations();
        }

        public IServiceCollection Services { get; }

        public Assembly[] Assemblies { get; }

        public GenericSearchBuilder UseConventions(Action<ConventionOptions> optionsAction = null)
        {
            Services.Configure<GenericSearchOptions>(options =>
                                                     {
                                                         options.ConventionOptions.UseConventions = true;
                                                         optionsAction?.Invoke(options.ConventionOptions);
                                                     });
            return this;
        }

        public GenericSearchBuilder ConfigureOptions(Action<GenericSearchOptions> optionsAction = null)
        {
            if (optionsAction == null)
            {
                Services.AddOptions<GenericSearchOptions>();
                return this;
            }

            Services.Configure(optionsAction);
            return this;
        }

        private void AddGenericSearchServices()
        {
            Services.AddHttpContextAccessor();

            Services.AddSingleton<ISearchConfigurationProvider, SearchConfigurationProvider>();
            Services.AddSingleton<IPropertyProcessorProvider, DefaultPropertyProcessorProvider>();
            Services.AddSingleton<IQueryStringTransformer, QueryStringTransformer>();
            Services.AddSingleton<IRouteValueTransformer, RouteValueTransformer>();
            Services.AddSingleton<IRequestModelCacheProvider, RequestModelCacheProvider>();
            Services.AddSingleton<IResultBinder, GenericSearchResultBinder>();
            
            Services.AddScoped<IGenericSearch, GenericSearch>();
            Services.AddScoped<IRequestModelCache, RequestModelCache>();
        }

        private void AddMvcServices()
        {
            Services.AddMvcCore(ConfigureMvcOptions);
        }

        private void ConfigureMvcOptions(MvcOptions options)
        {
            var fallbackModelBinderProvider = options.ModelBinderProviders.First(x => x is ComplexTypeModelBinderProvider);
            options.ModelBinderProviders.Insert(0, new GenericSearchModelBinderProvider(fallbackModelBinderProvider));
            options.Filters.Add<RedirectPostRequestActionFilter>();
            options.Filters.Add<TransferPropertiesFilter>();
        }

        private void AddConfigurations()
        {
            var types = Assemblies.SelectMany(a => a.GetTypes())
                                  .Where(x => x.GetInterfaces().Contains(typeof(ISearchConfiguration)));
            foreach (var type in types)
            {
                Services.AddSingleton(typeof(ISearchConfiguration), type);
            }
        }
    }
}