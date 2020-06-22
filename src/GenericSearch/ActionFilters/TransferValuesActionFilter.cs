using System;
using System.Threading.Tasks;
using GenericSearch.Configuration;
using GenericSearch.Internal;
using GenericSearch.Internal.Extensions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GenericSearch.ActionFilters
{
    public class TransferValuesActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly IModelProvider modelProvider;
        private readonly IListConfigurationProvider configurationProvider;

        public TransferValuesActionFilter(IModelProvider modelProvider, IListConfigurationProvider configurationProvider)
        {
            this.modelProvider = modelProvider;
            this.configurationProvider = configurationProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            var resultModel = result.Result.GetModel();
            if (resultModel == null)
            {
                return;
            }

            var requestModel = modelProvider.Provide();
            if (requestModel == null)
            {
                return;
            }

            var configuration = configurationProvider.GetConfiguration(requestModel.GetType());
            if (configuration == null)
            {
                return;
            }


            if (configuration.ResultType != resultModel.GetType())
            {
                return;
            }


            if (!IsSatisfiedByAction(configuration, context.ActionDescriptor))
            {
                return;
            }

            if (!configuration.PostRedirectGetConfiguration.Enabled)
            {
                return;
            }

            foreach (var searchConfiguration in configuration.SearchConfigurations)
            {
                var searchValue = searchConfiguration.RequestProperty.GetValue(requestModel);
                searchConfiguration.ResultProperty.SetValue(resultModel, searchValue);
            }

            foreach (var propertyConfiguration in configuration.PropertyConfigurations)
            {
                var propertyValue = propertyConfiguration.RequestProperty.GetValue(requestModel);
                propertyConfiguration.ResultProperty?.SetValue(resultModel, propertyValue);
            }

            var pageValue = configuration.PageConfiguration.RequestProperty?.GetValue(requestModel);
            configuration.PageConfiguration.ResultProperty?.SetValue(resultModel, pageValue);

            var rowsValue = configuration.RowsConfiguration.RequestProperty?.GetValue(requestModel);
            configuration.RowsConfiguration.ResultProperty?.SetValue(resultModel, rowsValue);

            var sortColumnValue = configuration.SortColumnConfiguration.RequestProperty?.GetValue(requestModel);
            configuration.SortColumnConfiguration.ResultProperty?.SetValue(resultModel, sortColumnValue);

            var sortDirectionValue = configuration.SortDirectionConfiguration.RequestProperty?.GetValue(requestModel);
            configuration.SortDirectionConfiguration.ResultProperty?.SetValue(resultModel, sortDirectionValue);
        }

        
        public static bool IsSatisfiedByAction(IListConfiguration listConfiguration, ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.RouteValues["action"]
                    .Equals(listConfiguration.TransferValuesConfiguration.ActionName, StringComparison.OrdinalIgnoreCase) &&
                !actionDescriptor.RouteValues["action"]
                    .Equals(listConfiguration.TransferValuesConfiguration.ActionName + "Async", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public int Order => 0;
    }
}