using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch;
using GenericSearch.Configuration;
using GenericSearch.Internal;
using GenericSearch.Internal.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper SelectList Extension methods for GenericSearch
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HtmlHelperSelectListExtensions
    {
        /// <summary>
        /// Helper method to retrieve a Select List from ViewData using an index key.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IReadOnlyList<SelectListItem> GetSelectList(this IHtmlHelper html, string key)
        {
            var selectList = html.ViewData[key];
            
            if (selectList is IReadOnlyList<SelectListItem> readyOnlySelectList)
            {
                return readyOnlySelectList;
            }

            if (selectList is IEnumerable<SelectListItem> selectListItemEnumerable)
            {
                return selectListItemEnumerable.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Helper method to retrieve a Select List from <see cref="IHtmlHelper{TModel}.ViewData"/> using the current model property name as the <see cref="ViewDataDictionary"/> index key.
        /// <para>
        ///     If the property is decorated with an <see cref="OptionSelectListAttribute"/> then the value of <see cref="OptionSelectListAttribute.ViewDataKey"/>
        ///     will be used as the index key of <see cref="ViewDataDictionary"/>.
        /// </para>
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IReadOnlyList<SelectListItem> GetSelectListForModel(this IHtmlHelper html)
        {
            var modelProperty = html.GetPropertyInfoForModel();
            var optionSelectListAttribute = modelProperty.GetCustomAttribute<OptionSelectListAttribute>();
            if (optionSelectListAttribute != null && !string.IsNullOrWhiteSpace(optionSelectListAttribute.ViewDataKey))
            {
                return html.GetSelectList(optionSelectListAttribute.ViewDataKey);
            }

            return html.GetSelectList(modelProperty.Name);
        }

        public static IReadOnlyList<SelectListItem> GetPropertiesSelectListForModel(this IHtmlHelper html)
        {
            var modelProvider = html.GetRequestService<IRequestModelProvider>();
            var model = modelProvider.GetCurrentRequestModel();

            if (model == null)
            {
                throw new NullReferenceException($"Unable to provide request model.");
            }

            var configurationProvider = html.GetRequestService<IListConfigurationProvider>();
            var configuration = configurationProvider.GetConfiguration(model.GetType());

            if (configuration == null)
            {
                throw new NullReferenceException($"Unable to find list configuration for '{model.GetType().FullName}'.");
            }

            return html.GetPropertiesSelectList(configuration.ItemType);
        }

        public static IReadOnlyList<SelectListItem> GetPropertiesSelectList<TModel, T>(this IHtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<T>>> expression)
        {
            return html.GetPropertiesSelectList(typeof(T));
        }

        /// <summary>
        /// Helper method to retrieve a Select List from ViewData using the specified model property name as the index key.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IReadOnlyList<SelectListItem> GetSelectListFor<T>(this IHtmlHelper<T> html, Expression<Func<T, object>> expression)
        {
            var propertyName = expression.GetPropertyInfo().Name;
            return html.GetSelectList(propertyName);
        }

        /// <summary>
        /// Helper method to retrieve a Select List of the properties in the provided type which have
        /// the <see cref="DisplayAttribute"/>, using the Display Name as the Text and the Name in lowercase as the value.
        /// </summary>
        /// <typeparam name="T">Entity which properties to make into select list</typeparam>
        /// <param name="html">Html helper</param>
        /// <returns>ReadOnlyList of select list items</returns>
        public static IReadOnlyList<SelectListItem> GetPropertiesSelectList<T>(this IHtmlHelper html)
        {
            var entityType = typeof(T);
            
            return entityType.GetProperties()
                             .Where(x => x.HasAttribute<DisplayAttribute>())
                             .Select(x => new SelectListItem(x.GetDisplayName(), x.Name.ToLowerInvariant()))
                             .ToArray();
        }

        /// <summary>
        /// Helper method to retrieve a Select List of the properties in the provided type which have
        /// the <see cref="DisplayAttribute"/>, using the Display Name as the Text and the Name in lowercase as the value.
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="type">Type which properties to make into select list</param>
        /// <returns>ReadOnlyList of select list items</returns>
        public static IReadOnlyList<SelectListItem> GetPropertiesSelectList(this IHtmlHelper html, Type type)
        {
            return type.GetProperties()
                .Where(x => x.HasAttribute<DisplayAttribute>())
                .Select(x => new SelectListItem(x.GetDisplayName(), x.Name.ToLowerInvariant()))
                .ToArray();
        }


    }
}