using System.Reflection;
using GenericSearch;
using GenericSearch.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service Collection Extensions for GenericSearchs
    /// </summary>
    public static class GenericSearchServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GenericSearch using default convention settings.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to find <see cref="IGenericSearchProfile"/> types from</param>
        /// <returns>Generic Search Builder</returns>
        public static IGenericSearchBuilder AddDefaultGenericSearch(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddGenericSearch(assemblies).UseConventions();
        }

        /// <summary>
        /// Adds GenericSearch and attempts to find <see cref="IGenericSearchProfile"/> types from provided assemblies.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to find <see cref="IGenericSearchProfile"/> types from</param>
        /// <returns>Generic Search Builder</returns>
        public static IGenericSearchBuilder AddGenericSearch(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddGenericSearch().AddProfilesFromAssemblies(assemblies);
        }

        /// <summary>
        /// Adds GenericSearch to services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>Generic Search Builder</returns>
        public static IGenericSearchBuilder AddGenericSearch(this IServiceCollection services)
        {
            var builder = new GenericSearchBuilder(services);
            return builder;
        }
    }
}