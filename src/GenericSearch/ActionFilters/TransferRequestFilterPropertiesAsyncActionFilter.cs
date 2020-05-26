using System.Collections.Generic;
using System.Threading.Tasks;
using GenericSearch.Configuration;
using GenericSearch.Configuration.Internal.Caching;
using GenericSearch.Extensions;
using GenericSearch.Internal.Extensions;
using GenericSearch.Providers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GenericSearch.ActionFilters
{
    /// <summary>
    /// Transfers the values of filter properties from request/parameter model to result/viewmodel
    /// </summary>
    public class TransferRequestFilterPropertiesAsyncActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly IFilterConfigurationProvider filterConfigurationProvider;
        private readonly IModelCacheProvider modelCacheProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="TransferRequestFilterPropertiesAsyncActionFilter"/>
        /// </summary>
        /// <param name="filterConfigurationProvider">Filter configuration provider</param>
        /// <param name="modelCacheProvider"></param>
        public TransferRequestFilterPropertiesAsyncActionFilter(IFilterConfigurationProvider filterConfigurationProvider, IModelCacheProvider modelCacheProvider)
        {
            this.filterConfigurationProvider = filterConfigurationProvider;
            this.modelCacheProvider = modelCacheProvider;
        }
        
        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            
            var resultModel = result.Result.GetModel();
            if (resultModel == null)
            {
                return;
            }

            var modelCache = modelCacheProvider.Provide();
            if (modelCache.ModelType == null)
            {
                return;
            }

            var configuration = filterConfigurationProvider.Provide(modelCache.ModelType);
            if (configuration == null)
            {
                return;
            }

            var resultModelType = resultModel.GetType();

            // Only transfer filter properties if the model type matches the one in configuration
            if (configuration.ResultType != resultModelType)
            {
                return;
            }

            if (!configuration.IsSatisfiedByAction(context.ActionDescriptor))
            {
                return;
            }

            if (configuration.IsCopyRequestFilterValuesDisabled())
            {
                return;
            }
            
            var requestModel = modelCache.Model;

            TransferSearchProperties(requestModel, resultModel, configuration.SearchConfigurations);
            TransferPageProperties(requestModel, resultModel, configuration.PageConfiguration);
            TransferSortProperties(requestModel, resultModel, configuration.SortConfiguration);
        }

        private void TransferSearchProperties(object requestModel, object resultModel, IEnumerable<ISearchConfiguration> configurations)
        {
            foreach (var property in configurations)
            {
                var search = property.RequestProperty.GetValue(requestModel);
                property.ResultProperty.SetValue(resultModel, search);
            }
        }

        private void TransferPageProperties(object requestModel, object resultModel, IPageConfiguration configuration)
        {
            var pageNumber = configuration.RequestPageNumberProperty?.GetValue(requestModel);
            configuration.ResultPageNumberProperty?.SetValue(resultModel, pageNumber);
            var rows = configuration.RequestRowsProperty?.GetValue(requestModel);
            configuration.ResultRowsProperty?.SetValue(resultModel, rows);
        }

        private void TransferSortProperties(object requestModel, object resultModel, ISortConfiguration configuration)
        {
            var sortProperty = configuration.RequestSortProperty?.GetValue(requestModel);
            configuration.ResultSortProperty?.SetValue(resultModel, sortProperty);
            var sortDirection = configuration.RequestSortDirection?.GetValue(requestModel);
            configuration.ResultSortDirection?.SetValue(resultModel, sortDirection);
        }

        /// <inheritdoc />
        public int Order => 0;
    }
}
