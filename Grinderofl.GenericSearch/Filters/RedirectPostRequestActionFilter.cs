#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Transformers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.Filters
{
    public class RedirectPostRequestActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ISearchConfigurationProvider configurationProvider;
        private readonly IRouteValueTransformer routeValueTransformer;
        private readonly ILogger<RedirectPostRequestActionFilter> logger;
        private readonly GenericSearchOptions options;

        public RedirectPostRequestActionFilter([NotNull] ISearchConfigurationProvider configurationProvider,
                                            [NotNull] IOptions<GenericSearchOptions> options,
                                            [NotNull] IRouteValueTransformer routeValueTransformer,
                                            [NotNull] ILogger<RedirectPostRequestActionFilter> logger)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            this.routeValueTransformer = routeValueTransformer ?? throw new ArgumentNullException(nameof(routeValueTransformer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (RedirectCore(context))
            {
                return;
            }

            await next();
        }

        private static readonly string[] AllowedActionNames = new[]
        {
            "Index",
            "IndexAsync"
        };

        private bool RedirectCore(ActionExecutingContext context)
        {
            // Return early if it's not post request
            if (!context.HttpContext.IsPostRequest())
            {
                logger.LogDebug($"Redirect failed: request method is '{context.HttpContext.GetRequestMethod()}'.");
                return false;
            }

            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor))
            {
                logger.LogDebug($"Redirect failed: ActionDescriptor is '{context.ActionDescriptor.GetType().FullName}'.");
                return false;
            }

            if (!AllowedActionNames.Contains(actionDescriptor.ActionName, StringComparer.InvariantCultureIgnoreCase))
            {
                logger.LogDebug($"Redirect failed: ActionName is '{actionDescriptor.ActionName}'.");
                return false;
            }

            // Return early if there's no configuration
            var configuration = configurationProvider.ForRequestParametersType(actionDescriptor.Parameters);
            if (configuration == null)
            {
                logger.LogDebug($"Redirect failed: Could not find an instance of '{nameof(ISearchConfiguration)}' with 'RequestType' parameter matching any of the 'ActionDescriptor' parameters.");
                return false;
            }

            if (!ShouldRedirectPostRequest(configuration))
            {
                return false;
            }

            var requestParameter = actionDescriptor.GetParameterDescriptor(configuration.RequestType);
            var model = context.ActionArguments[requestParameter.Name];
            var routeValues = routeValueTransformer.Transform(model);
            context.Result = new RedirectToActionResult(actionDescriptor.ActionName, actionDescriptor.ControllerName, routeValues);
            logger.LogDebug($"Redirecting request type '{configuration.RequestType.FullName}' ...");
            return true;
        }


        private bool ShouldRedirectPostRequest(ISearchConfiguration configuration)
        {
            if (options.RedirectPostRequests)
            {
                if (configuration.RedirectBehaviour == ProfileBehaviour.Disabled)
                {
                    logger.LogDebug($"Redirect failed: '{nameof(configuration.RedirectBehaviour)}' is 'Disabled' for the request type '{configuration.RequestType.FullName}'.");
                    return false;
                }

                return true;
            }

            if (configuration.RedirectBehaviour != ProfileBehaviour.Enabled || configuration.RedirectBehaviour == ProfileBehaviour.Default)
            {
                logger.LogDebug($"Redirect failed: '{nameof(options.RedirectPostRequests)}' is 'false' and '{nameof(configuration.RedirectBehaviour)}' is not 'Enabled' for the request type '{configuration.RequestType.FullName}'.");
                return false;
            }

            return true;
        }

        public int Order => 1;
    }
}