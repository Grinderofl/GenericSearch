#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.Filters
{
    public class TransferPropertiesFilter : IAsyncActionFilter, IAsyncPageFilter, IOrderedFilter
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly GenericSearchOptions options;

        public TransferPropertiesFilter(ISearchConfigurationProvider configurationProvider, IOptions<GenericSearchOptions> options)
        {
            this.configurationProvider = configurationProvider;
            this.options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            var model = GetModel(result.Result);
            if (model == null)
            {
                return;
            }

            var parameters = context.ActionDescriptor.Parameters;
            var configuration = configurationProvider.ForRequestParametersAndResultType(context.ActionDescriptor.Parameters, model.GetType());
            if (!ShouldTransferProperties(configuration))
            {
                return;
            }

            var parameterDescriptor = parameters.First(x => x.ParameterType == configuration.RequestType);
            var request = context.ActionArguments[parameterDescriptor.Name];

            TransferProperties(request, model, configuration);
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var result = await next();

            var model = GetModel(result.Result);
            if (model == null)
            {
                return;
            }

            var parameters = context.ActionDescriptor.Parameters;
            var configuration = configurationProvider.ForRequestParametersAndResultType(context.ActionDescriptor.Parameters, model.GetType());
            if (!ShouldTransferProperties(configuration))
            {
                return;
            }

            var parameterDescriptor = parameters.First(x => x.ParameterType == configuration.RequestType);
            var request = context.HandlerArguments[parameterDescriptor.Name];

            TransferProperties(request, model, configuration);
        }

        private static object GetModel(IActionResult actionResult)
        {
            return actionResult switch
            {
                ViewResult result => result.Model,
                PartialViewResult result => result.Model,
                PageResult result => result.Model,
                JsonResult result => result.Value,
                ViewComponentResult result => result.Model,
                _ => null
            };
        }

        private bool ShouldTransferProperties(ISearchConfiguration configuration)
        {
            if (options.TransferRequestProperties)
            {
                return configuration.TransferBehaviour != ProfileBehaviour.Disabled;
            }

            return configuration.TransferBehaviour == ProfileBehaviour.Enabled;
        }

        private void TransferProperties(object query, object model, ISearchConfiguration configuration)
        {
            TransferSearchProperties(query, model, configuration.SearchExpressions);
            //TransferSearchProperties(query, model, configuration.CustomSearchExpressions);
            TransferPagingProperties(query, model, configuration.PageExpression);
            TransferSortProperties(query, model, configuration.SortExpression);
            TransferOtherProperties(query, model, configuration.TransferExpressions);
        }

        protected virtual void TransferSearchProperties(object query, object model, IEnumerable<ISearchExpression> searchExpressions)
        {
            foreach (var searchExpression in searchExpressions)
            {
                var queryPropertyValue = searchExpression.RequestProperty.GetValue(query);
                searchExpression.ResultProperty.SetValue(model, queryPropertyValue);
            }
        }

        protected virtual void TransferPagingProperties(object query, object model, IPageExpression pageExpression)
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

        protected virtual void TransferSortProperties(object query, object model, ISortExpression sortExpression)
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

        protected virtual void TransferOtherProperties(object query, object model, IEnumerable<ITransferExpression> transferExpressions)
        {
            foreach (var transferExpression in transferExpressions)
            {
                var queryPropertyValue = transferExpression.RequestProperty.GetValue(query);
                transferExpression.ResultProperty.SetValue(model, queryPropertyValue);
            }
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public int Order => 0;
    }
}