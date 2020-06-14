using System.ComponentModel;
using GenericSearch.Searches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides a builder for GenericSearch Convention services
    /// </summary>
    public class GenericSearchConventionOptionsBuilder// : IGenericSearchConventionOptionsBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GenericSearchConventionOptionsBuilder"/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public GenericSearchConventionOptionsBuilder(IServiceCollection services)
        {
            Services = services;
            //Services.Configure<GenericSearchConventionOptions>(_ => { });
            Services.RemoveAll(typeof(IFilterConfigurationFactory));
            Services.AddSingleton<IFilterConfigurationFactory, ConventionFilterConfigurationFactory>();
        }

        private IServiceCollection Services { get; }
    }
}