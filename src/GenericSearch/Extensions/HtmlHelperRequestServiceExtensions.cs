using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper Request Services Extension methods for GenericSearch
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HtmlHelperRequestServiceExtensions
    {
        /// <summary>
        /// Helper method to get a required service from the current <see cref="HttpContext"/> <see cref="HttpContext.RequestServices"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static T GetRequestService<T>(this IHtmlHelper html)
        {
            return html.ViewContext.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}