using System;
using System.Collections.Generic;
using System.Reflection;
using GenericSearch.Configuration.Internal;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides a factory for creating finalized filter configurations
    /// </summary>
    public class DefaultFilterConfigurationFactory : IFilterConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultFilterConfigurationFactory"/>
        /// </summary>
        /// <param name="options"></param>
        public DefaultFilterConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// Creates a finalized filter configuration
        /// </summary>
        /// <param name="source">Original filter configuration from <see cref="IFilterConfiguration"/></param>
        /// <returns>Finalized instance of <see cref="IFilterConfiguration"/></returns>
        public IFilterConfiguration Create(IFilterConfiguration source)
        {
            return new FilterConfiguration(source)
            {
                SearchConfigurations = CreateSearchConfigurations(source.SearchConfigurations, source.ResultType),
                SortConfiguration = CreateSortConfiguration(source.SortConfiguration, source.ResultType, source.ItemType),
                PageConfiguration = CreatePageConfiguration(source.PageConfiguration, source.ResultType),
                CopyRequestFilterValuesConfiguration = CreateCopyRequestFilterValuesConfiguration(source.CopyRequestFilterValuesConfiguration),
                RedirectPostToGetConfiguration = CreateRedirectPostToGetConfiguration(source.RedirectPostToGetConfiguration),
                ListActionName = source.ListActionName ?? options.DefaultListActionName
            };
        }

        private IList<ISearchConfiguration> CreateSearchConfigurations(IList<ISearchConfiguration> searchConfigurations, Type resultType)
        {
            var result = new List<ISearchConfiguration>();

            foreach (var source in searchConfigurations)
            {
                var configuration = new SearchConfiguration(source.RequestProperty)
                {
                    ResultProperty = source?.ResultProperty ?? GetResultProperty(resultType, source.RequestProperty),
                    IsIgnored = source?.IsIgnored ?? false,
                    SearchFactory = source?.SearchFactory
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

        private ISortConfiguration CreateSortConfiguration(ISortConfiguration source, Type resultType, Type itemType)
        {
            var configuration = new SortConfiguration()
            {
                RequestSortProperty = source?.RequestSortProperty,
                RequestSortDirection = source?.RequestSortDirection,
                ResultSortProperty = source?.ResultSortProperty ?? resultType.GetPropertyInfo(source?.RequestSortProperty?.Name),
                ResultSortDirection = source?.ResultSortDirection ?? resultType.GetPropertyInfo(source?.RequestSortDirection?.Name),
                DefaultSortDirection = source?.GetDefaultSortDirection() ?? options.DefaultSortDirection
            };

            configuration.DefaultSortProperty = source?.DefaultSortProperty ?? itemType.GetPropertyInfo(configuration.RequestSortProperty?.Name);
            configuration.DefaultSortDirection = source?.DefaultSortDirection 
                                                 ?? configuration.RequestSortDirection?.GetDefaultValue<Direction?>() 
                                                 ?? options.DefaultSortDirection;

            return configuration;
        }

        private IPageConfiguration CreatePageConfiguration(IPageConfiguration source, Type resultType)
        {
            var configuration = new PageConfiguration()
            {
                RequestPageNumberProperty = source?.RequestPageNumberProperty,
                RequestRowsProperty = source?.RequestRowsProperty,
                ResultPageNumberProperty = source?.ResultPageNumberProperty ?? resultType.GetPropertyInfo(source?.ResultPageNumberProperty?.Name),
                ResultRowsProperty = source?.ResultRowsProperty ?? resultType.GetPropertyInfo(source?.ResultRowsProperty?.Name)
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