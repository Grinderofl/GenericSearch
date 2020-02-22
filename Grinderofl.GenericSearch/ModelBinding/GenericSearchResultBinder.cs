#pragma warning disable 1591
using System.Collections.Generic;
using System.Linq;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class GenericSearchResultBinder : IResultBinder
    {
        private readonly ISearchConfigurationProvider configurationProvider;

        public GenericSearchResultBinder(ISearchConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public virtual void BindResult(object request, object result, ISearchConfiguration configuration = null)
        {
            configuration ??= configurationProvider.ForRequestAndResultType(request.GetType(), result.GetType());
            
            BindSearchProperties(request, result, configuration.SearchExpressions.Union(configuration.CustomSearchExpressions));
            BindPagingProperties(request, result, configuration.PageExpression);
            BindSortProperties(request, result, configuration.SortExpression);
            BindOtherProperties(request, result, configuration.TransferExpressions);
        }

        protected virtual void BindSearchProperties(object query, object model, IEnumerable<ISearchExpression> searchExpressions)
        {
            foreach (var searchExpression in searchExpressions)
            {
                var queryPropertyValue = searchExpression.RequestProperty.GetValue(query);
                searchExpression.ResultProperty.SetValue(model, queryPropertyValue);
            }
        }

        protected virtual void BindPagingProperties(object query, object model, IPageExpression pageExpression)
        {
            if (pageExpression == null || pageExpression == NullPageExpression.Instance)
            {
                return;
            }

            var queryPageValue = pageExpression.RequestPageProperty.GetValue(query);
            pageExpression.ResultPageProperty.SetValue(model, queryPageValue);

            var queryRowsValue = pageExpression.RequestRowsProperty.GetValue(query);
            pageExpression.ResultRowsProperty.SetValue(model, queryRowsValue);
        }

        protected virtual void BindSortProperties(object query, object model, ISortExpression sortExpression)
        {
            if (sortExpression == null || sortExpression == NullSortExpression.Instance)
            {
                return;
            }

            var ordxValue = sortExpression.RequestSortByProperty.GetValue(query);
            sortExpression.ResultSortByProperty.SetValue(model, ordxValue);

            var orddValue = sortExpression.RequestSortDirectionProperty.GetValue(query);
            sortExpression.ResultSortDirectionProperty.SetValue(model, orddValue);
        }

        protected virtual void BindOtherProperties(object query, object model, IEnumerable<ITransferExpression> transferExpressions)
        {
            foreach (var transferExpression in transferExpressions)
            {
                var queryPropertyValue = transferExpression.RequestProperty.GetValue(query);
                transferExpression.ResultProperty.SetValue(model, queryPropertyValue);
            }
        }
    }
}