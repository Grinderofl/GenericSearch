using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GenericSearch.Exceptions;
using GenericSearch.Internal;
using GenericSearch.Internal.Configuration;
using GenericSearch.Searches;

namespace GenericSearch
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GenericSearch : IGenericSearch
    {
        protected IListConfigurationProvider ConfigurationProvider { get; }
        protected IRequestModelProvider RequestModelProvider { get; }

        public GenericSearch(IListConfigurationProvider configurationProvider, IRequestModelProvider requestModelProvider)
        {
            ConfigurationProvider = configurationProvider;
            RequestModelProvider = requestModelProvider;
        }

        protected virtual IListConfiguration GetRequestModelListConfiguration(object requestModel)
        {
            var requestType = requestModel.GetType();
            var configuration = ConfigurationProvider.GetConfiguration(requestType);

            if (configuration == null)
            {
                throw new MissingConfigurationException($"No configuration was defined for '{requestType.FullName}'");
            }

            return configuration;
        }

        protected virtual object GetCurrentRequestModel()
        {
            return RequestModelProvider.GetCurrentRequestModel();
        }

        public IQueryable<T> Search<T>(IQueryable<T> query, IListConfiguration configuration, object request)
        {
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

        public virtual IQueryable<T> Search<T>(IQueryable<T> query, object request)
        {
            var configuration = GetRequestModelListConfiguration(request);
            return Search(query, configuration, request);
        }
        
        public virtual IQueryable<T> Search<T>(IQueryable<T> query)
        {
            var request = GetCurrentRequestModel();
            return Search(query, request);
        }

        public IQueryable<T> Sort<T>(IQueryable<T> query, IListConfiguration configuration, object request)
        {
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

        public virtual IQueryable<T> Sort<T>(IQueryable<T> query, object request)
        {
            var configuration = GetRequestModelListConfiguration(request);
            return Sort(query, configuration, request);
        }

        public virtual IQueryable<T> Sort<T>(IQueryable<T> query)
        {
            var request = GetCurrentRequestModel();
            return Sort(query, request);
        }

        public IQueryable<T> Paginate<T>(IQueryable<T> query, IListConfiguration configuration, object request)
        {
            // TODO: Get Name property value from a querystringvalueModelProvider or something similar?

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

        public virtual IQueryable<T> Paginate<T>(IQueryable<T> query, object request)
        {
            var configuration = GetRequestModelListConfiguration(request);
            return Paginate(query, configuration, request);
        }

        public virtual IQueryable<T> Paginate<T>(IQueryable<T> query)
        {
            var request = GetCurrentRequestModel();
            return Paginate(query, request);
        }
    }
}