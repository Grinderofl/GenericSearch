using System.Diagnostics.CodeAnalysis;
using GenericSearch.Exceptions;
using GenericSearch.Internal;
using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper Paging Extension methods for GenericSearch
    /// </summary>
    [ExcludeFromCodeCoverage]
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
            var modelProvider = html.GetRequestService<IRequestModelProvider>();
            var model = modelProvider.GetCurrentRequestModel();
            if (model == null)
            {
                throw new ModelProviderException($"No request model was provided. Does the current action match the configured list action?");
            }

            var configurationProvider = html.GetRequestService<IListConfigurationProvider>();
            var configuration = configurationProvider.GetConfiguration(model.GetType());
            if (configuration == null)
            {
                throw new MissingConfigurationException($"Unable to find configuration for request model type '{model.GetType().FullName}'");
            }

            var pageConfiguration = configuration.PageConfiguration;

            var httpContext = html.ViewContext.HttpContext;
            var query = QueryHelpers.ParseQuery(httpContext.Request.QueryString.Value);
            
            var pageNumber = (pageConfiguration.RequestProperty?.Name ?? pageConfiguration.Name).ToLowerInvariant();
            var defaultPage = pageConfiguration.DefaultValue;

            var pageValue = page;
            if (pageValue == defaultPage)
            {
                query.Remove(pageNumber);
            }
            else
            {
                query[pageNumber] = pageValue.ToString();
            }

            return QueryString.Create(query).Value;
        }
    }
}