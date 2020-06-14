using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GenericSearch.Configuration.Internal.Caching;
using GenericSearch.Mvc;
using GenericSearch.Providers;
using GenericSearch.Searches;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Configuration
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

            Services.AddSingleton<FilterConfigurationCache>();
            Services.AddSingleton<IModelCacheProvider, ModelCacheProvider>();
            Services.AddSingleton<ISearchFactoryProvider, SearchFactoryProvider>();
            //Services.AddSingleton<IFilterConfigurationFactory, DefaultFilterConfigurationFactory>();
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
        /// Adds the provided type as a <see cref="IGenericSearchProfile"/> configuration.
        /// </summary>
        /// <typeparam name="T">Profile of type <see cref="GenericSearchProfile"/></typeparam>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder AddProfile<T>() where T : GenericSearchProfile
        {
            return AddProfile(typeof(T));
        }

        /// <summary>
        /// Adds the provided instance as a <see cref="IGenericSearchProfile"/> configuration
        /// </summary>
        /// <typeparam name="T">Profile of type <see cref="GenericSearchProfile"/></typeparam>
        /// <param name="profile">Instance of <typeparamref name="T"/></param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder AddProfile<T>(T profile) where T : GenericSearchProfile
        {
            Services.AddSingleton<IGenericSearchProfile>(profile);
            return this;
        }

        /// <summary>
        /// Adds the provided type <paramref name="profileType"/> a <see cref="IGenericSearchProfile"/> configuration.
        /// </summary>
        /// <param name="profileType">Type of the profile</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder AddProfile(Type profileType)
        {
            Services.AddSingleton(typeof(IGenericSearchProfile), profileType);
            return this;
        }

        /// <summary>
        /// Configures GenericSearch options
        /// </summary>
        /// <param name="optionsAction">Configuration action to perform</param>
        /// <returns>Generic Search Builder</returns>
        public IGenericSearchBuilder Configure(Action<GenericSearchOptions> optionsAction)
        {
            Services.Configure(optionsAction);
            return this;
        }
    }
}