using Grinderofl.GenericSearch.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper Paging Extension methods for GenericSearch
    /// </summary>
    public static class HtmlHelperPagingExtensions
    {
        /// <summary>
        /// Helper method to generate URL for a specific page using the current query string.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetUrlForPage(this IHtmlHelper html, int page)
        {
            var resultType = html.ViewData.Model.GetType();
            var configurationProvider = html.GetRequestService<ISearchConfigurationProvider>();
            var configuration = configurationProvider.ForResultType(resultType);

            var httpContext = html.ViewContext.HttpContext;
            var query = QueryHelpers.ParseQuery(httpContext.Request.QueryString.Value);

            var pageProperty = configuration.PageExpression.RequestPageProperty.Name.ToLowerInvariant();
            var defaultPage = configuration.PageExpression.DefaultPage;

            if (page == defaultPage)
            {
                query.Remove(pageProperty);
            }
            else
            {
                query[pageProperty] = page.ToString();
            }

            return QueryString.Create(query).Value;
        }
    }
}