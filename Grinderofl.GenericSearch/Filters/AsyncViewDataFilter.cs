#pragma warning disable 1591
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.Filters
{
    /// <summary>
    /// Base class to modify ViewData after action has executed
    /// </summary>
    public abstract class AsyncViewDataFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            var viewData = (context.Controller as Controller)?.ViewData;

            if (viewData != null)
            {
                await ModifyViewDataAsync(viewData, context.HttpContext.RequestAborted);
            }
        }

        protected abstract Task ModifyViewDataAsync(ViewDataDictionary viewData, CancellationToken cancellationToken);
    }
}
