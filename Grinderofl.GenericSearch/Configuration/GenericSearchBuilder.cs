using System;
using System.Linq;
using System.Reflection;
using Grinderofl.GenericSearch.Configuration.Internal.Caching;
using Grinderofl.GenericSearch.Internal;
using Grinderofl.GenericSearch.Mvc;
using Grinderofl.GenericSearch.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides a builder for GenericSearch services
    /// </summary>
    public class GenericSearchBuilder : IGenericSearchBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GenericSearchBuilder"/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public GenericSearchBuilder(IServiceCollection services)
        {
            Services = services;
            Services.Configure<GenericSearchOptions>(_ => { });
            Services.ConfigureOptions<ConfigureGenericSearchMvcOptions>();

            Services.AddSingleton<IModelCacheProvider, ModelCacheProvider>();
            Services.AddSingleton<ISearchFactoryProvider, SearchFactoryProvider>();
            Services.AddSingleton<IFilterConfigurationFactory, DefaultFilterConfigurationFactory>();
            Services.AddSingleton<IFilterConfigurationProvider, FilterConfigurationProvider>();
            Services.AddScoped<ModelCache>();
            Services.AddScoped<IGenericSearch, GenericSearch>();
        }

        /// <summary>
        /// Service collection
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Applies configuration from all types implementing <see cref="IGenericSearchProfile"/> to GenericSearch.
        /// </summary>
        /// <param name="assemblies">Assemblies to find <see cref="IGenericSearchProfile"/> types from</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder AddProfilesFromAssemblies(params Assembly[] assemblies)
        {
            var filterProfiles = assemblies.SelectMany(x => x.GetTypes())
                                           .Where(x => x.GetInterfaces()
                                                        .Contains(typeof(IGenericSearchProfile)));
            
            foreach (var type in filterProfiles)
            {
                Services.AddSingleton(typeof(IGenericSearchProfile), type);
            }

            return this;
        }

        /// <summary>
        /// Applies configuration from all types implementing <see cref="IGenericSearchProfile"/> to GenericSearch
        /// from assembly containing the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type which' containing assembly to search </typeparam>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder AddProfilesFromAssemblyOf<T>()
        {
            return AddProfilesFromAssemblies(typeof(T).Assembly);
        }

        /// <summary>
        /// Configures GenericSearch to use convention based defaults
        /// </summary>
        /// <param name="conventionOptionsBuilderAction">Action to configure convention options builder</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder UseConventions(Action<IGenericSearchConventionOptionsBuilder> conventionOptionsBuilderAction = null)
        {
            var builder = new GenericSearchConventionOptionsBuilder(Services);
            conventionOptionsBuilderAction?.Invoke(builder);
            return this;
        }

        /// <summary>
        /// Specifies whether POST request should be redirected to GET globally
        /// </summary>
        /// <param name="redirectPostToGet">Sets whether to redirect POST to GET</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder RedirectPostToGet(bool redirectPostToGet)
        {
            Services.Configure<GenericSearchOptions>(x => x.RedirectPostToGet = redirectPostToGet);
            return this;
        }

        /// <summary>
        /// Specifies whether filter values should be copied from request to result after
        /// an action has finished executing
        /// </summary>
        /// <param name="copyRequestFilterValues">Sets whether to copy request filter values to result</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder CopyRequestFilterValues(bool copyRequestFilterValues)
        {
            Services.Configure<GenericSearchOptions>(x => x.CopyRequestFilterValues = copyRequestFilterValues);
            return this;
        }

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against.
        /// <remarks>
        /// The default name is 'Index'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">Default name to use</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder DefaultListActionName(string defaultName)
        {
            Services.Configure<GenericSearchOptions>(x => x.DefaultListActionName = defaultName);
            return this;
        }

        /// <summary>
        /// Specifies the default number of rows per page
        /// </summary>
        /// <param name="defaultRows">Default number of rows</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder DefaultRowsPerPage(int defaultRows)
        {
            Services.Configure<GenericSearchOptions>(x => x.DefaultRowsPerPage = defaultRows);
            return this;
        }
    }
}