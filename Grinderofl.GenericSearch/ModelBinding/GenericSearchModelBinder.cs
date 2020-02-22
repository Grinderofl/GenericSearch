#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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

            BindSearchProperties(configuration.SearchExpressions.Union(configuration.CustomSearchExpressions), bindingContext.Model);
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

            sortExpression.RequestSortDirectionProperty.SetValue(model, sortExpression.DefaultSortDirection);

            if (sortExpression.DefaultSortBy != null)
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

            if (pageExpression.DefaultRows > 0)
            {
                pageExpression.RequestRowsProperty.SetValue(model, pageExpression.DefaultRows);
            }

            pageExpression.RequestPageProperty.SetValue(model, pageExpression.DefaultPage);
        }
    }


}
