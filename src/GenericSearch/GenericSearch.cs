using System;
using System.Linq;
using GenericSearch.Configuration.Internal.Caching;
using GenericSearch.Exceptions;
using GenericSearch.Helpers;
using GenericSearch.Providers;
using GenericSearch.Searches;

namespace GenericSearch
{
    /// <summary>
    /// Provides a generic way of searching, sorting, and paginating queryable types
    /// </summary>
    public class GenericSearch : IGenericSearch
    {
        private readonly IFilterConfigurationProvider configurationProvider;
        private readonly IModelCacheProvider modelCacheProvider;
        
        /// <summary>
        /// Initializes a new instance of <see cref="GenericSearch"/>
        /// </summary>
        /// <param name="configurationProvider">Filter Configuration Provider</param>
        /// <param name="modelCacheProvider">Model Cache Provider</param>
        public GenericSearch(IFilterConfigurationProvider configurationProvider, IModelCacheProvider modelCacheProvider)
        {
            this.configurationProvider = configurationProvider;
            this.modelCacheProvider = modelCacheProvider;
        }

        /// <summary>
        /// Filters the <paramref name="query"/> results using the provided request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IQueryable<T> Search<T>(IQueryable<T> query, object request)
        {
            var requestType = request.GetType();
            var configuration = configurationProvider.Provide(requestType);

            if (configuration == null)
            {
                return query;
            }
            
            foreach (var searchConfiguration in configuration.SearchConfigurations.Where(x => !x.IsIgnored))
            {
                var search = (ISearch) searchConfiguration.RequestProperty.GetValue(request);
                query = search.ApplyToQuery(query);
            }

            return query;
        }

        /// <summary>
        /// Filters the <paramref name="query"/> results using a cached request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<T> Search<T>(IQueryable<T> query)
        {
            var modelCache = modelCacheProvider.Provide();
            return Search(query, modelCache.Model);
        }

        /// <summary>
        /// Sorts the <paramref name="query"/> results using the provided request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IQueryable<T> Sort<T>(IQueryable<T> query, object request)
        {
            var requestType = request.GetType();
            var configuration = configurationProvider.Provide(requestType);

            if (configuration == null)
            {
                return query;
            }

            var sortPropertyName = (string)configuration.SortConfiguration.RequestSortProperty.GetValue(request);
            if (string.IsNullOrWhiteSpace(sortPropertyName))
            {
                return query;
            }

            var sortDirection = (Direction)configuration.SortConfiguration.RequestSortDirection.GetValue(request);

            var propertyExpression = ExpressionFactory.Create<T>(sortPropertyName);

            return sortDirection == Direction.Ascending
                       ? query.OrderBy(propertyExpression)
                       : query.OrderByDescending(propertyExpression);
        }

        /// <summary>
        /// Sorts the <paramref name="query"/> results using a cached request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<T> Sort<T>(IQueryable<T> query)
        {
            return Sort(query, modelCacheProvider.Provide().Model);
        }

        /// <summary>
        /// Pages the <paramref name="query"/> results using the provided request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MissingConfigurationException">Configuration for <paramref name="request"/> type was not found.</exception>
        /// <exception cref="NullReferenceException">Page number is null on <paramref name="request"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Page number is 0 on <paramref name="request"/></exception>
        /// <exception cref="NullReferenceException">Row count is null on <paramref name="request"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Row count is 0 on <paramref name="request"/></exception>
        public IQueryable<T> Paginate<T>(IQueryable<T> query, object request)
        {
            var requestType = request.GetType();
            var configuration = configurationProvider.Provide(requestType);

            if (configuration == null)
            {
                throw new MissingConfigurationException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': filter configuration was not found for '{requestType.FullName}'.");
            }

            if (configuration.PageConfiguration == null)
            {
                throw new MissingConfigurationException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': Pagination is not configured for '{requestType.FullName}'.");
            }

            var page = (int?)configuration.PageConfiguration.RequestPageNumberProperty?.GetValue(request);

            // [Issue] https://dev.azure.com/sulenero/GenericSearch/_workitems/edit/9/
            var rows = (int?)configuration.PageConfiguration.RequestRowsProperty?.GetValue(request);
            
            
            if (page == null)
            {
                throw new NullReferenceException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': Page number is null on '{requestType.FullName}'.");
            }

            if (page == 0)
            {
                throw new ArgumentOutOfRangeException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': Page number is 0 on '{requestType.FullName}'.");
            }

            if (rows == null)
            {
                throw new NullReferenceException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': Row count is null on '{requestType.FullName}'.");
            }

            if (rows == 0)
            {
                throw new ArgumentOutOfRangeException($"Unable to paginate 'IQueryable<{typeof(T).FullName}>': Row count is 0 on '{requestType.FullName}'.");
            }

            var skip = (page.Value - 1) * rows.Value;
            return query.Skip(skip).Take(rows.Value);
        }

        /// <summary>
        /// Pages the <paramref name="query"/> results using a cached request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<T> Paginate<T>(IQueryable<T> query)
        {
            return Paginate(query, modelCacheProvider.Provide().Model);
        }
    }
}