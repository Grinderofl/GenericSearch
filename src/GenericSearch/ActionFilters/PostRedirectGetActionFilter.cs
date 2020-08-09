using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericSearch.ActionFilters.Visitors;
using GenericSearch.Internal;
using GenericSearch.Internal.Configuration;
using GenericSearch.Internal.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters
{
    public class PostRedirectGetActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly IRequestModelProvider requestModelProvider;
        private readonly IListConfigurationProvider configurationProvider;

        public PostRedirectGetActionFilter(IRequestModelProvider requestModelProvider,
                                           IListConfigurationProvider configurationProvider)
        {
            this.requestModelProvider = requestModelProvider;
            this.configurationProvider = configurationProvider;
        }

        public int Order => 1;

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

            var model = requestModelProvider.GetCurrentRequestModel();
            if (model == null)
            {
                return false;
            }


            var configuration = configurationProvider.GetConfiguration(model.GetType());
            if (configuration == null)
            {
                return false;
            }

            if (!configuration.PostRedirectGetConfiguration
                .ActionName
                .Equals(descriptor.ActionName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!configuration.PostRedirectGetConfiguration.Enabled)
            {
                return false;
            }

            var rvd = new RouteValueDictionary();
            var visitor = new ModelPropertyVisitor(configuration, model, rvd);
            foreach (var property in configuration.RequestType.GetProperties())
            {
                visitor.Visit(property);
            }

            context.Result = new RedirectToActionResult(descriptor.ActionName, descriptor.ControllerName, rvd);
            return true;
        }

    }
}
