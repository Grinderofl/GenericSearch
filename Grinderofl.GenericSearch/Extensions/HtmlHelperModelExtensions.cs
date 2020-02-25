using System.ComponentModel.DataAnnotations;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper Model Extension methods for GenericSearch
    /// </summary>
    public static class HtmlHelperModelExtensions
    {
        /// <summary>
        /// Helper method to retrieve the placeholder (<see cref="DisplayAttribute.Prompt"/>) of the current model property
        /// as declared by its parent.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static string GetPlaceholderForModel(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewData.ModelMetadata.Placeholder;
        }

        /// <summary>
        /// Helper method to retrieve the name of the current model property as declared by its parent.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static string GetPropertyNameForModel(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.ViewData.ModelMetadata.PropertyName;
        }

        /// <summary>
        /// Helper method to retrieve the Display Name of the current model property. More reliable than @Html.DisplayNameForModel().
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetDisplayNameForModel(this IHtmlHelper html)
        {
            return html.ViewData.ModelMetadata.DisplayName ?? html.ViewData.ModelMetadata.Name;
        }

        /// <summary>
        /// Helper method to retrieve the <see cref="PropertyInfo"/> of the current model property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfoForModel(this IHtmlHelper htmlHelper)
        {
            var propertyName = htmlHelper.GetPropertyNameForModel();
            return htmlHelper.ViewData.ModelMetadata.ContainerType.GetProperty(propertyName);
        }
    }
}