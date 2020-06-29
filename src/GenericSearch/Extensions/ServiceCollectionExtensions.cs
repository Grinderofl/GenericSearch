using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using GenericSearch.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Extensions
{
    public static class ServiceCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IGenericSearchServicesBuilder AddGenericSearch(this IServiceCollection services, params Assembly[] assemblies)
        {
            var builder = new GenericSearchServicesBuilder(services);
            foreach (var assembly in assemblies)
            {
                builder.AddProfilesFromAssembly(assembly);
            }

            return builder;
        }


        public static IGenericSearchServicesBuilder AddDefaultGenericSearch(this IServiceCollection services, Action<GenericSearchOptions> configureOptions, params Assembly[] assemblies)
        {
            var builder = new GenericSearchServicesBuilder(services)
                .AddDefaultServices()
                .AddDefaultActivators()
                .AddModelBinder()
                .AddPostToGetRedirects()
                .ConfigureOptions(configureOptions);

            foreach (var assembly in assemblies)
            {
                builder.AddProfilesFromAssembly(assembly);
            }

            return builder;
        }

        public static IGenericSearchServicesBuilder AddDefaultGenericSearch(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddDefaultGenericSearch(_ => { }, assemblies);
        }
    }
}
