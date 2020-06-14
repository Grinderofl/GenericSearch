using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;

namespace GenericSearch.Definition.Expressions
{
    public class ListExpression<TRequest, TItem, TResult> : IListExpression<TRequest, TItem, TResult>, IListDefinition
    {
        public Type RequestType => typeof(TRequest);
        public Type ItemType => typeof(TItem);
        public Type ResultType => typeof(TResult);

        public Dictionary<PropertyInfo, ISearchDefinition> SearchDefinitions { get; } = new Dictionary<PropertyInfo, ISearchDefinition>();
        public IPageDefinition PageDefinition { get; private set; }
        public IRowsDefinition RowsDefinition { get; private set; }
        public ISortColumnDefinition SortColumnDefinition { get; private set; }
        public ISortDirectionDefinition SortDirectionDefinition { get; private set; }
        
        public PropertyInfo[] RequestProperties => RequestType.GetProperties();
        public PropertyInfo[] ResultProperties => ResultType.GetProperties();

        public Dictionary<PropertyInfo, IPropertyDefinition> PropertyDefinitions { get; } = new Dictionary<PropertyInfo, IPropertyDefinition>();
        public IPostRedirectGetDefinition PostRedirectGetDefinition { get; set; }
        public ITransferValuesDefinition TransferValuesDefinition { get; set; }
        public IRequestFactoryDefinition RequestFactoryDefinition { get; set; }

        public IListExpression<TRequest, TItem, TResult> Search(Expression<Func<TRequest, ISearch>> property, Action<ISearchExpression<TRequest, TItem, TResult>> action = null)
        {
            var expression = new SearchExpression<TRequest, TItem, TResult>(property);
            action?.Invoke(expression);
            SearchDefinitions.Add(property.GetPropertyInfo(), expression);
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Property<T>(Expression<Func<TRequest, T>> property, Action<IPropertyExpression<TRequest, T, TResult>> action = null)
        {
            var expression = new PropertyExpression<TRequest, T, TResult>(property);
            action?.Invoke(expression);
            PropertyDefinitions.Add(property.GetPropertyInfo(), expression);
            return this;
        }


        public IListExpression<TRequest, TItem, TResult> SortColumn(Expression<Func<TRequest, string>> property, Action<ISortColumnExpression<TRequest, TItem, TResult>> action = null)
        {
            var expression = new SortColumnExpression<TRequest, TItem, TResult>(property);
            action?.Invoke(expression);
            SortColumnDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> SortColumn(string name = null, Action<ISortColumnExpression<TRequest, TItem, TResult>> action = null)
        {
            name = !string.IsNullOrWhiteSpace(name) ? name : null;
            var expression = new SortColumnExpression<TRequest, TItem, TResult>(name);
            action?.Invoke(expression);
            SortColumnDefinition = expression;
            return this;
        }
        

        public IListExpression<TRequest, TItem, TResult> SortDirection(Expression<Func<TRequest, Direction>> property, Action<ISortDirectionExpression<TRequest, TResult>> action = null)
        {
            var expression = new SortDirectionExpression<TRequest, TResult>(property);
            action?.Invoke(expression);
            SortDirectionDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> SortDirection(string name = null, Action<ISortDirectionExpression<TRequest, TResult>> action = null)
        {
            var expression = new SortDirectionExpression<TRequest, TResult>(name);
            action?.Invoke(expression);
            SortDirectionDefinition = expression;
            return this;
        }


        public IListExpression<TRequest, TItem, TResult> Page(Expression<Func<TRequest, int>> property, Action<IPageExpression<TRequest, TResult>> action = null)
        {
            var expression = new PageExpression<TRequest, TResult>(property);
            action?.Invoke(expression);
            PageDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Page(string name, Action<IPageExpression> action = null)
        {
            var expression = new PageExpression<TRequest, TResult>(name);
            action?.Invoke(expression);
            PageDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Page(int? defaultValue = null)
        {
            var expression = new PageExpression<TRequest, TResult>(defaultValue);
            PageDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Rows(Expression<Func<TRequest, int>> property, Action<IRowsExpression<TRequest, TResult>> action = null)
        {
            var expression = new RowsExpression<TRequest, TResult>(property);
            action?.Invoke(expression);
            RowsDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Rows(string name, Action<IRowsExpression> action = null)
        {
            var expression = new RowsExpression<TRequest, TResult>(name);
            action?.Invoke(expression);
            RowsDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> Rows(int? defaultValue = null)
        {
            var expression = new RowsExpression<TRequest, TResult>();
            ((IRowsExpression) expression).DefaultValue(defaultValue);
            RowsDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> PostRedirectGet(Action<IPostRedirectGetExpression> action)
        {
            var expression = new PostRedirectGetExpression();
            action.Invoke(expression);
            PostRedirectGetDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> TransferValues(Action<ITransferValuesExpression> action)
        {
            var expression = new TransferValuesExpression();
            action.Invoke(expression);
            TransferValuesDefinition = expression;
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<object> factoryMethod)
        {
            RequestFactoryDefinition = new RequestFactoryExpression(factoryMethod);
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> ConstructUsing<T>() where T : IRequestFactory
        {
            RequestFactoryDefinition = new RequestFactoryExpression(typeof(T));
            return this;
        }

        public IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<IServiceProvider, object> activator)
        {
            RequestFactoryDefinition = new RequestFactoryExpression(activator);
            return this;
        }
    }
}