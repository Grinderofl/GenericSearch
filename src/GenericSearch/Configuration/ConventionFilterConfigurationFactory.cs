﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Configuration.Internal;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Extensions;
using GenericSearch.Providers;
using GenericSearch.Searches;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides a factory for creating finalized filter configurations using conventions
    /// </summary>
    public class ConventionFilterConfigurationFactory : IFilterConfigurationFactory
    {
        private readonly ISearchFactoryProvider searchFactoryProvider;
        private readonly GenericSearchOptions options;
        private readonly GenericSearchConventionOptions conventionOptions;

        /// <summary>
        /// Initializes a new instance of <see cref="ConventionFilterConfigurationFactory"/>
        /// </summary>
        /// <param name="searchFactoryProvider"></param>
        /// <param name="conventionOptions"></param>
        /// <param name="options"></param>
        public ConventionFilterConfigurationFactory(ISearchFactoryProvider searchFactoryProvider, 
                                                    IOptions<GenericSearchConventionOptions> conventionOptions, 
                                                    IOptions<GenericSearchOptions> options)
        {
            this.searchFactoryProvider = searchFactoryProvider;
            this.options = options.Value;
            this.conventionOptions = conventionOptions.Value;
        }

        /// <summary>
        /// Creates a finalized filter configuration
        /// </summary>
        /// <param name="source">Original filter configuration from <see cref="IGenericSearchProfile"/></param>
        /// <returns>Finalized instance of <see cref="IFilterConfiguration"/></returns>
        public IFilterConfiguration Create(IFilterConfiguration source)
        {
            return new FilterConfiguration(source)
            {
                SearchConfigurations = CreateSearchConfigurations(source.SearchConfigurations, source.RequestType, source.ResultType),
                SortConfiguration = CreateSortConfiguration(source.SortConfiguration, source.RequestType, source.ResultType, source.ItemType),
                PageConfiguration = CreatePageConfiguration(source.PageConfiguration, source.RequestType, source.ResultType),
                CopyRequestFilterValuesConfiguration = CreateCopyRequestFilterValuesConfiguration(source.CopyRequestFilterValuesConfiguration),
                RedirectPostToGetConfiguration = CreateRedirectPostToGetConfiguration(source.RedirectPostToGetConfiguration),
                ListActionName = source.ListActionName ?? options.DefaultListActionName
            };
        }

        private IList<ISearchConfiguration> CreateSearchConfigurations(IList<ISearchConfiguration> searchConfigurations, Type requestType, Type resultType)
        {
            var result = new List<ISearchConfiguration>();

            var searchProperties = requestType.GetSearchProperties();
            foreach (var searchProperty in searchProperties)
            {
                var source = searchConfigurations.FirstOrDefault(x => x.RequestProperty == searchProperty);

                var configuration = new SearchConfiguration(searchProperty)
                {
                    ResultProperty = source?.ResultProperty ?? GetResultProperty(resultType, searchProperty),
                    IsIgnored = source?.IsIgnored ?? false,
                    SearchFactory = source?.SearchFactory ?? searchFactoryProvider.Provide(searchProperty)
                };

                result.Add(configuration);
            }

            return result;
        }

        private static PropertyInfo GetResultProperty(Type resultType, PropertyInfo requestProperty)
        {
            var resultProperty = resultType.GetProperty(requestProperty.Name);
            if (resultProperty == null)
            {
                throw new PropertyNotFoundException($"Search Property '{requestProperty.PropertyType.FullName}' was not found on '{resultType.FullName}'");
            }

            return resultProperty;
        }

        private ISortConfiguration CreateSortConfiguration(ISortConfiguration source, Type requestType, Type resultType, Type itemType)
        {
            var configuration = new SortConfiguration
            {
                RequestSortProperty = source?.RequestSortProperty ?? requestType.GetPropertyInfo(conventionOptions.DefaultSortOrderPropertyName),
                RequestSortDirection = source?.RequestSortDirection ?? requestType.GetPropertyInfo(conventionOptions.DefaultSortDirectionPropertyName)
            };
            
            configuration.ResultSortProperty = source?.ResultSortProperty ?? resultType.GetPropertyInfo(configuration.RequestSortProperty?.Name);
            configuration.ResultSortDirection = source?.ResultSortDirection ?? resultType.GetPropertyInfo(configuration.RequestSortDirection?.Name);

            configuration.DefaultSortProperty = source?.DefaultSortProperty ?? itemType.GetPropertyInfo(configuration.RequestSortProperty?.GetDefaultValue<string>());
            configuration.DefaultSortDirection = source?.DefaultSortDirection ?? configuration.RequestSortDirection?.GetDefaultValue<Direction?>() ?? options.DefaultSortDirection;
            return configuration;
        }

        private IPageConfiguration CreatePageConfiguration(IPageConfiguration source, Type requestType, Type resultType)
        {
            var configuration = new PageConfiguration
            {
                RequestPageNumberProperty = source?.RequestPageNumberProperty ?? requestType.GetProperty(conventionOptions.DefaultPageNumberPropertyName),
                RequestRowsProperty = source?.RequestRowsProperty ?? requestType.GetProperty(conventionOptions.DefaultRowsPropertyName),
                ResultPageNumberProperty = source?.ResultPageNumberProperty ?? resultType.GetProperty(source?.ResultPageNumberProperty?.Name ?? conventionOptions.DefaultPageNumberPropertyName),
                ResultRowsProperty = source?.ResultRowsProperty ?? resultType.GetProperty(source?.ResultRowsProperty?.Name ?? conventionOptions.DefaultRowsPropertyName)
            };

            if (configuration.RequestRowsProperty != null)
            {
                configuration.DefaultRowsPerPage = source?.DefaultRowsPerPage
                                                   ?? configuration.RequestRowsProperty?.GetDefaultValue<int?>()
                                                   ?? options.DefaultRowsPerPage;
            }

            if (configuration.RequestPageNumberProperty != null)
            {
                configuration.DefaultPageNumber = source?.DefaultPageNumber
                                                  ?? configuration.RequestPageNumberProperty.GetDefaultValue<int?>()
                                                  ?? options.DefaultPageNumber;
            }

            return configuration;
        }

        private ICopyRequestFilterValuesConfiguration CreateCopyRequestFilterValuesConfiguration(ICopyRequestFilterValuesConfiguration source)
        {
            return new CopyRequestFilterValuesConfiguration()
            {
                ActionName = source?.ActionName ?? options.DefaultListActionName,
                ConfigurationState = CreateConfigurationState(source?.ConfigurationState ?? ConfigurationState.Default, options.CopyRequestFilterValues)
            };
        }

        private IRedirectPostToGetConfiguration CreateRedirectPostToGetConfiguration(IRedirectPostToGetConfiguration source)
        {
            return new RedirectPostToGetConfiguration()
            {
                ActionName = source?.ActionName ?? options.DefaultListActionName,
                ConfigurationState = CreateConfigurationState(source?.ConfigurationState ?? ConfigurationState.Default, options.RedirectPostToGet)
            };
        }

        private ConfigurationState CreateConfigurationState(ConfigurationState local, bool global)
        {
            // If it's globally enabled, then it'll only be disabled if it's locally disabled
            if (global)
            {
                return local == ConfigurationState.Disabled ? ConfigurationState.Disabled : ConfigurationState.Enabled;
            }

            return local == ConfigurationState.Enabled ? ConfigurationState.Enabled : ConfigurationState.Disabled;
        }
    }
}