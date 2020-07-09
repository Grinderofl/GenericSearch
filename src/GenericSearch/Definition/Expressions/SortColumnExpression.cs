using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Definition.Expressions
{
    public class SortColumnExpression<TRequest, TEntity, TResult> : ISortColumnDefinition, ISortColumnExpression<TRequest, TEntity, TResult>
    {
        public SortColumnExpression(Expression<Func<TRequest, string>> requestProperty)
        {
            RequestProperty = requestProperty.GetPropertyInfo();
            DefaultValue = RequestProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        }

        public SortColumnExpression(string name = null) => Name = !string.IsNullOrWhiteSpace(name) ? name : null;

        public PropertyInfo RequestProperty { get; }
        public string Name { get; }
        public PropertyInfo ResultProperty { get; private set; }
        public PropertyInfo DefaultProperty { get; private set; }
        public object DefaultValue { get; private set; }
        public ISortColumnExpression<TRequest, TEntity, TResult> MapTo(Expression<Func<TResult, string>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        ISortColumnExpression<TRequest, TEntity, TResult> ISortColumnExpression<TRequest, TEntity, TResult>.DefaultTo(Expression<Func<TEntity, object>> defaultValue)
        {
            DefaultProperty = defaultValue.GetPropertyInfo();
            return this;
        }

        ISortColumnExpression<TRequest, TEntity, TResult> ISortColumnExpression<TRequest, TEntity, TResult>.DefaultValue(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
    }
}