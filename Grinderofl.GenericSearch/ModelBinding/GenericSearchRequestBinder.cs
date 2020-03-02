#pragma warning disable 1591
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class GenericSearchRequestBinder : IRequestBinder
    {
        public void BindRequest(object request, ISearchConfiguration configuration)
        {
            BindSearchProperties(configuration.SearchExpressions
                .Union(configuration.CustomSearchExpressions)
                .Union(configuration.IgnoredSearchExpressions), request);
            BindSortProperties(configuration.SortExpression, request);
            BindPageProperties(configuration.PageExpression, request);
        }

        private static readonly PropertyInfo PropertyName = typeof(AbstractSearch)
            .GetProperty(nameof(AbstractSearch.Property), BindingFlags.NonPublic |
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.Static);

        private void BindSearchProperties(IEnumerable<ISearchExpression> searchExpressions, object model)
        {
            foreach (var searchExpression in searchExpressions)
            {
                var search = searchExpression.Search;

                var serialized = JsonSerializer.Serialize(search);
                var deserialized = JsonSerializer.Deserialize(serialized, search.GetType());

                ReplaceOptionalDefaultValue(searchExpression, deserialized);

                var value = PropertyName.GetValue(search);
                PropertyName.SetValue(deserialized, value);

                searchExpression.RequestProperty.SetValue(model, deserialized);
            }
        }

        private static void ReplaceOptionalDefaultValue(ISearchExpression searchExpression, object search)
        {
            var defaultValue = searchExpression.RequestProperty.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValue != null)
            {
                if (search is BooleanSearch bs && !bs.Is.Equals(defaultValue.Value))
                {
                    bs.Is = (bool) defaultValue.Value;
                }

                if (search is TrueBooleanSearch tbs && !tbs.Is.Equals(defaultValue.Value))
                {
                    tbs.Is = (bool) defaultValue.Value;
                }

                if (search is OptionalBooleanSearch obs && obs.Is != (bool?) defaultValue.Value)
                {
                    obs.Is = (bool?) defaultValue.Value;
                }
            }
        }

        private void BindSortProperties(ISortExpression sortExpression, object model)
        {
            if (sortExpression == null || sortExpression == NullSortExpression.Instance)
            {
                return;
            }

            var defaultSortDirectionValueAttribute = sortExpression.RequestSortDirectionProperty.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultSortDirectionValueAttribute != null)
            {
                sortExpression.RequestSortDirectionProperty.SetValue(model, defaultSortDirectionValueAttribute.Value);
            }
            else
            {
                sortExpression.RequestSortDirectionProperty.SetValue(model, sortExpression.DefaultSortDirection);
            }

            var defaultSortByValueAttribute = sortExpression.RequestSortByProperty.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultSortByValueAttribute != null)
            {
                sortExpression.RequestSortByProperty.SetValue(model, defaultSortByValueAttribute.Value);
            }
            else if (sortExpression.DefaultSortBy != null)
            {
                sortExpression.RequestSortByProperty.SetValue(model, sortExpression.DefaultSortBy.Name);
            }
        }

        private void BindPageProperties(IPageExpression pageExpression, object model)
        {
            if (pageExpression == null || pageExpression == NullPageExpression.Instance)
            {
                return;
            }

            var defaultRowsValueAttribute = pageExpression.RequestRowsProperty.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultRowsValueAttribute != null)
            {
                pageExpression.RequestRowsProperty.SetValue(model, defaultRowsValueAttribute.Value);
            }
            else if (pageExpression.DefaultRows > 0)
            {
                pageExpression.RequestRowsProperty.SetValue(model, pageExpression.DefaultRows);
            }

            var defaultPageValueAttribute = pageExpression.RequestPageProperty.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultPageValueAttribute != null)
            {
                pageExpression.RequestPageProperty.SetValue(model, defaultPageValueAttribute.Value);
            }
            else
            {
                pageExpression.RequestPageProperty.SetValue(model, pageExpression.DefaultPage);
            }
        }
    }
}