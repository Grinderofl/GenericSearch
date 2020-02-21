#pragma warning disable 1591
using Grinderofl.GenericSearch.Caching;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Helpers;
using System.Linq;

namespace Grinderofl.GenericSearch
{
    public class GenericSearch : IGenericSearch
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly IRequestModelCacheProvider cacheProvider;

        public GenericSearch(ISearchConfigurationProvider configurationProvider, IRequestModelCacheProvider cacheProvider)
        {
            this.configurationProvider = configurationProvider;
            this.cacheProvider = cacheProvider;
        }

        private IRequestModelCache cache;
        private IRequestModelCache Cache => cache ??= cacheProvider.Provide();

        public IQueryable<T> Search<T>(IQueryable<T> query, object request)
        {
            var entityType = typeof(T);
            var requestType = request.GetType();
            var configuration = configurationProvider.ForEntityAndRequestType(entityType, requestType);

            if (configuration == null)
            {
                return query;
            }

            foreach (var searchExpression in configuration.SearchExpressions)
            {
                query = searchExpression.Search.ApplyToQuery(query);
            }

            return query;
        }

        public IQueryable<T> Search<T>(IQueryable<T> query)
        {
            var request = Cache.Get();
            return Search(query, request);
        }

        public IQueryable<T> Sort<T>(IQueryable<T> query, object request)
        {
            var entityType = typeof(T);
            var requestType = request.GetType();
            var configuration = configurationProvider.ForEntityAndRequestType(entityType, requestType);

            if (configuration == null)
            {
                return query;
            }

            var sortPropertyName = (string)configuration.SortExpression.RequestSortByProperty.GetValue(request);
            if (string.IsNullOrWhiteSpace(sortPropertyName))
            {
                return query;
            }

            var sortDirection = (Direction)configuration.SortExpression.RequestSortDirectionProperty.GetValue(request);

            var propertyExpression = ExpressionFactory.Create<T>(sortPropertyName);

            return sortDirection == Direction.Ascending
                       ? query.OrderBy(propertyExpression)
                       : query.OrderByDescending(propertyExpression);
        }

        public IQueryable<T> Sort<T>(IQueryable<T> query)
        {
            var request = Cache.Get();
            return Sort(query, request);
        }

        public IQueryable<T> Paginate<T>(IQueryable<T> query, object request)
        {
            var entityType = typeof(T);
            var requestType = request.GetType();
            var configuration = configurationProvider.ForEntityAndRequestType(entityType, requestType);

            if (configuration == null)
            {
                return query;
            }

            var page = (int)configuration.PageExpression.RequestPageProperty.GetValue(request);
            var rows = (int)configuration.PageExpression.RequestRowsProperty.GetValue(request);

            var skip = (page - 1) * rows;
            return query.Skip(skip).Take(rows);
        }

        public IQueryable<T> Paginate<T>(IQueryable<T> query)
        {
            var request = Cache.Get();
            return Paginate(query, request);
        }
    }
}