using System;
using System.ComponentModel;
using System.Reflection;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Internal.Caching;
using Grinderofl.GenericSearch.Exceptions;
using Grinderofl.GenericSearch.Internal;
using Grinderofl.GenericSearch.Providers;
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
            var modelCacheProvider = html.GetRequestService<IModelCacheProvider>();
            var filterConfigurationProvider = html.GetRequestService<IFilterConfigurationProvider>();
            var modelCache = modelCacheProvider.Provide();
            if (modelCache.Model == null)
            {
                throw new ModelCacheException($"No model has been cached. Does the current action match the configured list action?");
            }

            var filterConfiguration = filterConfigurationProvider.Provide(modelCache.ModelType);
            
            var httpContext = html.ViewContext.HttpContext;
            var query = QueryHelpers.ParseQuery(httpContext.Request.QueryString.Value);
            
            var configuration = filterConfiguration.PageConfiguration;
            var pageNumber = configuration.RequestPageNumberProperty.Name.ToLowerInvariant();
            var defaultPage = configuration.DefaultPageNumber;

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