using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GenericSearch;
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
    public static class HtmlHelperPagingExtensions
    {
        /// <summary>
        /// Helper method to generate URL for a specific page using the current query string.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"><see cref="HttpRequest"/> is not accessible from the current <paramref name="htmlHelper"/>.</exception>
        /// <exception cref="NullReferenceException">Default implementation of <see cref="IListConfigurationProvider"/> has not been registered. See also <see cref="GenericSearchServicesBuilder.AddDefaultServices"/>.</exception>
        /// <exception cref="ModelProviderException">The query model was not found in the current request pipeline.</exception>
        /// <exception cref="MissingConfigurationException">Configuration for the current query model type was not found. See also <see cref="ListProfile"/>.</exception>
        /// <exception cref="NullReferenceException">Page Configuration for the current model type was not found.</exception>
        public static string GetUrlForPage(this IHtmlHelper htmlHelper, int page)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext?.Request;
            if (httpContext == null)
            {
                throw new NullReferenceException($"HttpRequest of the current HttpContext is null. Is the calling assembly an ASP.NET Core application?");
            }

            var modelProvider = htmlHelper.GetRequestService<IRequestModelProvider>();
            var model = modelProvider.GetCurrentRequestModel();
            if (model == null)
            {
                throw new ModelProviderException($"No request model was provided. Does the current action match the configured list action?");
            }

            var configurationProvider = htmlHelper.GetRequestService<IListConfigurationProvider>();
            if (configurationProvider == null)
            {
                throw new NullReferenceException($"Unable to resolve List Configuration Provider. Make sure default GenericSearch services have been registered.");
            }

            var configuration = configurationProvider.GetConfiguration(model.GetType());
            if (configuration == null)
            {
                throw new MissingConfigurationException($"Unable to find List Configuration for request model type '{model.GetType().FullName}'. Has a List Definition been created?");
            }

            var pageConfiguration = configuration.PageConfiguration;
            if (pageConfiguration == null)
            {
                throw new NullReferenceException($"Page Configuration is null. Is pagination enabled in GenericSearchOptions?");
            }

            var queryString = httpContext.QueryString.Value;
            var query = QueryHelpers.ParseQuery(queryString);
            var parameter = (pageConfiguration.RequestProperty?.Name ?? pageConfiguration.Name).ToLowerInvariant();
            
            if (page == pageConfiguration.DefaultValue)
            {
                query.Remove(parameter);
            }
            else
            {
                query[parameter] = page.ToString();
            }

            return QueryString.Create(query).Value;
        }
    }
}