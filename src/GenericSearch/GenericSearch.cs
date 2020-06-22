using System;
using System.Diagnostics;
using System.Linq;
using GenericSearch.Configuration;
using GenericSearch.Exceptions;
using GenericSearch.Internal;
using GenericSearch.Searches;

namespace GenericSearch
{
    public class GenericSearch : IGenericSearch
    {
        private readonly IListConfigurationProvider configurationProvider;
        private readonly IModelProvider provider;

        public GenericSearch(IListConfigurationProvider configurationProvider, IModelProvider provider)
        {
            this.configurationProvider = configurationProvider;
            this.provider = provider;
        }

        private IListConfiguration GetConfiguration(object request)
        {
            var requestType = request.GetType();
            var configuration = configurationProvider.GetConfiguration(requestType);

            if (configuration == null)
            {
                throw new MissingConfigurationException($"No configuration was defined for '{requestType.FullName}'");
            }

            return configuration;
        }

        public IQueryable<T> Search<T>(IQueryable<T> query, object request)
        {
            var configuration = GetConfiguration(request);

            foreach (var searchConfiguration in configuration.SearchConfigurations.Where(x => !x.Ignored))
            {
                var search = searchConfiguration.RequestProperty.GetValue(request) as ISearch;

                if (search == null)
                {
                    throw new NullReferenceException($"The '{searchConfiguration.RequestProperty.Name}' Search Property on '{request.GetType().FullName}' is null.");
                }

                if (search.IsActive())
                {
                    query = search.ApplyToQuery(query);
                }
            }

            return query;
        }
        
        public IQueryable<T> Search<T>(IQueryable<T> query)
        {
            var request = provider.Provide();
            return Search(query, request);
        }

        public IQueryable<T> Sort<T>(IQueryable<T> query, object request)
        {
            var configuration = GetConfiguration(request);

            var sortColumn = (string) configuration.SortColumnConfiguration?.RequestProperty?.GetValue(request);
            
            var sortDirection = (Direction?) configuration.SortDirectionConfiguration?.RequestProperty?.GetValue(request);

            if (string.IsNullOrWhiteSpace(sortColumn) || sortDirection == null)
            {
                return query;
            }

            var expression = ExpressionFactory.Create<T>(sortColumn);
            return sortDirection == Direction.Ascending
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);

        }

        public IQueryable<T> Sort<T>(IQueryable<T> query)
        {
            var request = provider.Provide();
            return Sort(query, request);
        }

        public IQueryable<T> Paginate<T>(IQueryable<T> query, object request)
        {
            var configuration = GetConfiguration(request);

            // TODO: Get Name property value from a querystringvalueprovider or something similar?

            var page = (int?) configuration.PageConfiguration?.RequestProperty?.GetValue(request);
            var rows = (int?) configuration.RowsConfiguration?.RequestProperty?.GetValue(request);

            if (page == null)
            {
                throw new NullReferenceException($"Unable to paginate: page is missing");
            }

            if (page == 0)
            {
                throw new ArgumentOutOfRangeException($"Unable to paginate: page is 0");
            }

            if (rows == null)
            {
                throw new NullReferenceException($"Unable to paginate: rows is missing");
            }

            if (rows == 0)
            {
                throw new ArgumentOutOfRangeException($"Unable to paginate: rows is 0");
            }

            var skip = (page.Value - 1) * rows.Value;
            return query.Skip(skip).Take(rows.Value);
        }

        public IQueryable<T> Paginate<T>(IQueryable<T> query)
        {
            var request = provider.Provide();
            return Paginate(query, request);
        }
    }
}