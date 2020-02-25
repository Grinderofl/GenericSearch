using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Grinderofl.GenericSearch.Attributes;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Processors;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// IHtmlHelper SelectList Extension methods for GenericSearch
    /// </summary>
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
        /// Helper method to retrieve a Select List of the properties in the provided type, using the
        /// Display Name as the Text and the Name in lowercase as the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IReadOnlyList<SelectListItem> GetPropertiesSelectList<T>(this IHtmlHelper html)
        {
            var entityType = typeof(T);
            var processorProvider = html.GetRequestService<IPropertyProcessorProvider>();
            var processor = processorProvider.ProvideForEntityType(entityType);

            return entityType.GetProperties()
                             .Where(x => !processor.ShouldIgnoreEntityProperty(x))
                             .Select(x => new SelectListItem(x.GetDisplayName(), x.Name.ToLowerInvariant()))
                             .ToArray();
        }
    }
}