using System;
using System.Threading.Tasks;
using Grinderofl.GenericSearch.ActionFilters.Visitors;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Internal.Caching;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Internal;
using Grinderofl.GenericSearch.Internal.Extensions;
using Grinderofl.GenericSearch.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Grinderofl.GenericSearch.ActionFilters
{
    /// <summary>
    /// Redirects POST requests to GET for List Actions which have a matching Generic Search model parameter
    /// </summary>
    public class RedirectPostToGetAsyncActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly IModelCacheProvider modelCacheProvider;
        private readonly IFilterConfigurationProvider configurationProvider;
        
        /// <summary>
        /// Initializes a new instance of <see cref="RedirectPostToGetAsyncActionFilter"/>
        /// </summary>
        /// <param name="modelCacheProvider"></param>
        /// <param name="configurationProvider"></param>
        public RedirectPostToGetAsyncActionFilter(IModelCacheProvider modelCacheProvider, IFilterConfigurationProvider configurationProvider)
        {
            this.modelCacheProvider = modelCacheProvider;
            this.configurationProvider = configurationProvider;
        }

        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (TryRedirect(context))
            {
                return;
            }

            await next();
        }

        private bool TryRedirect(ActionExecutingContext context)
        {
            if (!context.HttpContext.IsPostRequest())
            {
                return false;
            }
            
            if (!(context.ActionDescriptor is ControllerActionDescriptor descriptor))
            {
                return false;
            }

            var modelCache = modelCacheProvider.Provide();
            if (modelCache.ModelType == null)
            {
                return false;
            }

            var filterConfiguration = configurationProvider.Provide(modelCache.ModelType);
            if (filterConfiguration == null)
            {
                return false;
            }
            
            if (!filterConfiguration.IsSatisfiedByAction(descriptor))
            {
                return false;
            }

            if (filterConfiguration.IsRedirectPostToGetDisabled())
            {
                return false;
            }

            var routeValueDictionary = new RouteValueDictionary();
            var modelPropertyVisitor = new ModelPropertyVisitor(filterConfiguration, modelCache.Model, routeValueDictionary);

            foreach (var property in modelCache.ModelType.GetProperties())
            {
                modelPropertyVisitor.Visit(property);
            }

            context.Result = new RedirectToActionResult(descriptor.ActionName, descriptor.ControllerName, routeValueDictionary);
            return true;
        }

        /// <inheritdoc />
        public int Order => 1;
    }
}