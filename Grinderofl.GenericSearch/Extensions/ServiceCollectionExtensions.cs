using Grinderofl.GenericSearch.Configuration;
using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// IServiceCollection extension methods for GenericSearch
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GenericSearch services and registers search profiles from provided assemblies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="profileAssemblies"></param>
        /// <returns></returns>
        public static GenericSearchBuilder AddGenericSearch(this IServiceCollection services, params Assembly[] profileAssemblies)
        {
            return new GenericSearchBuilder(services, profileAssemblies).ConfigureOptions();
        }

        /// <summary>
        /// Adds GenericSearch services, configures its options, and registers search profiles from provided assemblies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <param name="profileAssemblies"></param>
        /// <returns></returns>
        public static GenericSearchBuilder AddGenericSearch(this IServiceCollection services, Action<GenericSearchOptions> optionsAction, params Assembly[] profileAssemblies)
        {
            return new GenericSearchBuilder(services, profileAssemblies).ConfigureOptions(optionsAction);
        }
    }
}
