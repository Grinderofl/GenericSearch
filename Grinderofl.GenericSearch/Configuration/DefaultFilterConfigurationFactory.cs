using System;
using System.Collections.Generic;
using System.Reflection;
using Grinderofl.GenericSearch.Configuration.Internal;
using Grinderofl.GenericSearch.Exceptions;
using Grinderofl.GenericSearch.Internal.Extensions;
using Microsoft.Extensions.Options;

namespace Grinderofl.GenericSearch.Configuration
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
                SortConfiguration = CreateSortConfiguration(source.SortConfiguration, source.ResultType),
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

        private ISortConfiguration CreateSortConfiguration(ISortConfiguration source, Type resultType)
        {
            return new SortConfiguration()
            {
                RequestSortProperty = source?.RequestSortProperty,
                RequestSortDirection = source?.RequestSortDirection,
                ResultSortProperty = source?.ResultSortProperty ?? (source?.RequestSortProperty != null ? resultType.GetProperty(source.RequestSortProperty.Name) : null),
                ResultSortDirection = source?.ResultSortDirection ?? (source?.RequestSortDirection != null ? resultType.GetProperty(source.RequestSortDirection.Name) : null),
                DefaultSortDirection = source?.GetDefaultSortDirection() ?? options.DefaultSortDirection
            };
        }

        private IPageConfiguration CreatePageConfiguration(IPageConfiguration source, Type resultType)
        {
            return new PageConfiguration()
            {
                RequestPageNumberProperty = source?.RequestPageNumberProperty,
                RequestRowsProperty = source?.RequestRowsProperty,
                ResultPageNumberProperty = source?.ResultPageNumberProperty ?? (source?.RequestPageNumberProperty != null ? resultType.GetProperty(source.ResultPageNumberProperty.Name) : null),
                ResultRowsProperty = source?.ResultRowsProperty ?? (source?.RequestRowsProperty != null ? resultType.GetProperty(source.ResultRowsProperty.Name) : null),
                DefaultRowsPerPage = source?.GetDefaultRowsPerPage() ?? options.DefaultRowsPerPage,
                DefaultPageNumber = source?.GetDefaultPageNumber() ?? options.DefaultPageNumber
            };
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