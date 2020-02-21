#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration.Expressions;
using Grinderofl.GenericSearch.Searches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration
{
    public sealed class ConventionSearchConfiguration : ISearchConfiguration
    {
        private readonly SearchConfigurationBase profile;
        private readonly GenericSearchOptions globalOptions;
        private readonly ConventionOptions options;

        public ConventionSearchConfiguration(SearchConfigurationBase profile, GenericSearchOptions options)
        {
            this.profile = profile;
            globalOptions = options;
            this.options = options.ConventionOptions;
            InitialiseConfiguration();
        }

        public Type EntityType => profile.EntityType;
        public Type RequestType => profile.RequestType;
        public Type ResultType => profile.ResultType;

        public IEnumerable<ISearchExpression> SearchExpressions { get; private set; }
        
        public IEnumerable<ITransferExpression> TransferExpressions { get; private set; }

        public ISortExpression SortExpression { get; private set; }

        public IPageExpression PageExpression { get; private set; }

        public ProfileBehaviour RedirectBehaviour { get; private set; }

        public ProfileBehaviour TransferBehaviour { get; private set; }

        private void InitialiseConfiguration()
        {
            SearchExpressions = InitialiseSearchExpressions();
            TransferExpressions = InitialiseTransferExpressions();
            SortExpression = InitialiseSortExpression();
            PageExpression = InitialisePageExpression();
            RedirectBehaviour = InitialiseRedirectBehaviour();
            TransferBehaviour = InitialiseTransferBehaviour();
        }

        private IEnumerable<ISearchExpression> InitialiseSearchExpressions()
        {
            if (profile.SearchExpressions != null)
            {
                return profile.SearchExpressions;
            }

            return CreateSearchExpressions().ToArray();
        }

        private IEnumerable<ISearchExpression> CreateSearchExpressions()
        {
            var requestProperties = FindSearchProperties(RequestType);

            foreach (var requestProperty in requestProperties)
            {
                var resultProperty = ResultType.GetProperty(requestProperty.Name) ??
                                     throw PropertyNullException.Create(ResultType, requestProperty.Name, nameof(requestProperty));
                var search = SearchFactory.Create(requestProperty);
                yield return new ConventionSearchExpression(search, requestProperty, resultProperty);
            }
        }

        private IEnumerable<ITransferExpression> InitialiseTransferExpressions()
        {
            if (profile.TransferExpressions != null)
            {
                return profile.TransferExpressions;
            }

            return Enumerable.Empty<ITransferExpression>();
        }

        private ISortExpression InitialiseSortExpression()
        {
            if (profile.SortExpression != null)
            {
                return profile.SortExpression;
            }

            var requestSortByProperty = RequestType.GetProperty(options.SortByPropertyName);
            var requestSortDirectionProperty = RequestType.GetProperty(options.SortDirectionPropertyName);

            if (requestSortByProperty == null || requestSortDirectionProperty == null)
            {
                return NullSortExpression.Instance;
            }

            var resultSortByProperty = ResultType.GetProperty(options.SortByPropertyName) ??
                                       throw PropertyNullException.Create(ResultType, options.SortByPropertyName, nameof(options.SortByPropertyName));
            var resultSortDirectionProperty = ResultType.GetProperty(options.SortDirectionPropertyName) ??
                                              throw PropertyNullException.Create(ResultType, options.SortDirectionPropertyName, nameof(options.SortDirectionPropertyName));

            return new ConventionSortExpression(requestSortByProperty, requestSortDirectionProperty, resultSortByProperty, resultSortDirectionProperty, options.DefaultSortDirection);
        }

        private IPageExpression InitialisePageExpression()
        {
            if (profile.PageExpression != null)
            {
                return profile.PageExpression;
            }

            var requestPageProperty = RequestType.GetProperty(options.PagePropertyName);
            var requestRowsProperty = RequestType.GetProperty(options.RowsPropertyName);

            if (requestPageProperty == null || requestRowsProperty == null)
            {
                return NullPageExpression.Instance;
            }

            var resultPageProperty = ResultType.GetProperty(options.PagePropertyName) ??
                                     throw PropertyNullException.Create(ResultType, options.PagePropertyName, nameof(options.PagePropertyName));
            var resultRowsProperty = ResultType.GetProperty(options.RowsPropertyName) ??
                                     throw PropertyNullException.Create(ResultType, options.RowsPropertyName, nameof(options.RowsPropertyName));

            return new ConventionPageExpression(requestPageProperty, requestRowsProperty, resultPageProperty, resultRowsProperty, options.DefaultRows, options.DefaultPage);
        }

        private ProfileBehaviour InitialiseRedirectBehaviour()
        {
            if (profile.RedirectBehaviour == ProfileBehaviour.Default)
            {
                return globalOptions.RedirectPostRequests
                           ? ProfileBehaviour.Enabled
                           : ProfileBehaviour.Disabled;
            }

            return profile.RedirectBehaviour;
        }

        private ProfileBehaviour InitialiseTransferBehaviour()
        {
            if (profile.TransferBehaviour == ProfileBehaviour.Default)
            {
                return globalOptions.TransferRequestProperties
                           ? ProfileBehaviour.Enabled
                           : ProfileBehaviour.Disabled;
            }

            return profile.TransferBehaviour;
        }

        private static IEnumerable<PropertyInfo> FindSearchProperties(Type type)
        {
            return type.GetProperties().Where(x => x.PropertyType.GetInterfaces().Contains(typeof(ISearch)));
        }
    }
}