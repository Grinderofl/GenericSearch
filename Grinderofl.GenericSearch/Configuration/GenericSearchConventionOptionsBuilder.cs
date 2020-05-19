﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides a builder for GenericSearch Convention services
    /// </summary>
    public class GenericSearchConventionOptionsBuilder : IGenericSearchConventionOptionsBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GenericSearchConventionOptionsBuilder"/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public GenericSearchConventionOptionsBuilder(IServiceCollection services)
        {
            Services = services;
            Services.Configure<GenericSearchConventionOptions>(_ => { });
            Services.RemoveAll(typeof(IFilterConfigurationFactory));
            Services.AddSingleton<IFilterConfigurationFactory, ConventionFilterConfigurationFactory>();
        }

        private IServiceCollection Services { get; }

        /// <summary>
        /// Specifies the property name to use for Sort Order when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordx'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        public GenericSearchConventionOptionsBuilder DefaultSortOrderPropertyName(string defaultName)
        {
            Services.Configure<GenericSearchConventionOptions>(x => x.DefaultSortOrderPropertyName = defaultName);
            return this;
        }

        /// <summary>
        /// Specifies the property name to use for Sort Direction when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordd'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        public GenericSearchConventionOptionsBuilder DefaultSortDirectionPropertyName(string defaultName)
        {
            Services.Configure<GenericSearchConventionOptions>(x => x.DefaultSortDirectionPropertyName = defaultName);
            return this;
        }

        /// <summary>
        /// Specifies the property name to use for Page number when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Page'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        public GenericSearchConventionOptionsBuilder DefaultPagePropertyName(string defaultName)
        {
            Services.Configure<GenericSearchConventionOptions>(x => x.DefaultPageNumberPropertyName = defaultName);
            return this;
        }

        /// <summary>
        /// Specifies the property name to use for Row count when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Rows'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        public GenericSearchConventionOptionsBuilder DefaultRowsPropertyName(string defaultName)
        {
            Services.Configure<GenericSearchConventionOptions>(x => x.DefaultRowsPropertyName = defaultName);
            return this;
        }
    }
}