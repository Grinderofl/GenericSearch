#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class GenericSearchModelBinder : IModelBinder
    {
        private readonly ISearchConfiguration configuration;
        private readonly IModelBinder fallbackModelBinder;

        public GenericSearchModelBinder(ISearchConfiguration configuration, IModelBinder fallbackModelBinder)
        {
            this.configuration = configuration;
            this.fallbackModelBinder = fallbackModelBinder;
        }

        public virtual async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.Model == null)
            {
                bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
            }

            BindSearchProperties(configuration.SearchExpressions, bindingContext.Model);
            BindSortProperties(configuration.SortExpression, bindingContext.Model);
            BindPageProperties(configuration.PageExpression, bindingContext.Model);

            await fallbackModelBinder.BindModelAsync(bindingContext);
        }

        private void BindSearchProperties(IEnumerable<ISearchExpression> searchExpressions, object model)
        {
            foreach (var searchExpression in searchExpressions)
            {
                searchExpression.RequestProperty.SetValue(model, searchExpression.Search);
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
