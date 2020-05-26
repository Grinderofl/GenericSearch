using System.Linq;
using GenericSearch.Configuration.Internal.Caching;
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
        public IQueryable<T> Paginate<T>(IQueryable<T> query, object request)
        {
            var requestType = request.GetType();
            var configuration = configurationProvider.Provide(requestType);

            if (configuration == null)
            {
                return query;
            }

            var page = (int)configuration.PageConfiguration.RequestPageNumberProperty.GetValue(request);
            var rows = (int)configuration.PageConfiguration.RequestRowsProperty.GetValue(request);

            var skip = (page - 1) * rows;
            return query.Skip(skip).Take(rows);
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