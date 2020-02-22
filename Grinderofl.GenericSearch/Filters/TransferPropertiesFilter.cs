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
using Grinderofl.GenericSearch.ModelBinding;

namespace Grinderofl.GenericSearch.Filters
{
    public class TransferPropertiesFilter : IAsyncActionFilter, IAsyncPageFilter, IOrderedFilter
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly IResultBinder resultBinder;
        private readonly GenericSearchOptions options;

        public TransferPropertiesFilter(ISearchConfigurationProvider configurationProvider, IOptions<GenericSearchOptions> options, IResultBinder resultBinder)
        {
            this.configurationProvider = configurationProvider;
            this.resultBinder = resultBinder;
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
            var parameterDescriptor = parameters.FirstOrDefault(x => configurationProvider.HasRequestType(x.ParameterType));
            if (parameterDescriptor == null)
            {
                return;
            }

            var configuration = configurationProvider.ForRequestAndResultType(parameterDescriptor.ParameterType, model.GetType());
            if (!ShouldBindProperties(configuration))
            {
                return;
            }

            var request = context.ActionArguments[parameterDescriptor.Name];
            resultBinder.BindResult(request, model, configuration);
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
            var parameterDescriptor = parameters.FirstOrDefault(x => configurationProvider.HasRequestType(x.ParameterType));
            if (parameterDescriptor == null)
            {
                return;
            }

            var configuration = configurationProvider.ForRequestAndResultType(parameterDescriptor.ParameterType, model.GetType());
            if (!ShouldBindProperties(configuration))
            {
                return;
            }

            var request = context.HandlerArguments[parameterDescriptor.Name];
            resultBinder.BindResult(request, result, configuration);
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

        private bool ShouldBindProperties(ISearchConfiguration configuration)
        {
            if (options.TransferRequestProperties)
            {
                return configuration.TransferBehaviour != ProfileBehaviour.Disabled;
            }

            return configuration.TransferBehaviour == ProfileBehaviour.Enabled;
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public int Order => 0;
    }
}