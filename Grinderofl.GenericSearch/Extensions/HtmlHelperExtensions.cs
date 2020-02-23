using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Processors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper Extension methods for GenericSearch
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Helper method to retrieve the name of the current model property as declared by its parent
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static string GetModelPropertyName(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.ViewData.ModelMetadata.PropertyName;
        }

        /// <summary>
        /// Helper method to retrieve a Select List from ViewData using an index key
        /// </summary>
        /// <param name="html"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(this IHtmlHelper html, string key)
        {
            return html.ViewData[key] as IEnumerable<SelectListItem>;
        }

        /// <summary>
        /// Helper method to retrieve the Display Name of the current model property. More reliable than @Html.DisplayNameForModel().
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetModelDisplayName(this IHtmlHelper html)
        {
            return html.ViewData.ModelMetadata.DisplayName ?? html.ViewData.ModelMetadata.Name;
        }

        /// <summary>
        /// Helper method to retrieve a Select List from ViewData using the current model property name as the index key
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetModelSelectList(this IHtmlHelper html)
        {
            var propertyName = html.GetModelPropertyName();
            return html.GetSelectList(propertyName);
        }

        /// <summary>
        /// Helper method to generate URL for a specific page using the current query string
        /// </summary>
        /// <param name="html"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetPageUrl(this IHtmlHelper html, int page)
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

        /// <summary>
        /// Helper method to retrieve a Select List of the properties in the provided type, using the
        /// Display Name as the Text and the Name in lowercase as the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetPropertySelectList<T>(this IHtmlHelper html)
        {
            var entityType = typeof(T);
            var processorProvider = html.GetRequestService<IPropertyProcessorProvider>();
            var processor = processorProvider.ProvideForEntityType(entityType);

            return entityType.GetProperties()
                             .Where(x => !processor.ShouldIgnoreEntityProperty(x))
                             .Select(x => new SelectListItem(x.GetDisplayName(), x.Name.ToLowerInvariant()))
                             .ToList();
        }

        internal static T GetRequestService<T>(this IHtmlHelper html)
        {
            return html.ViewContext.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}