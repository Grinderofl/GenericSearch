using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Definition.Expressions
{
    public class PageExpression<TRequest, TResult> : IPageExpression, IPageExpression<TRequest, TResult>, IPageDefinition
    {
        public PageExpression(Expression<Func<TRequest, int>> property)
        {
            RequestProperty = property.GetPropertyInfo();
            DefaultValue = (int?) RequestProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        }

        public PageExpression(string name) => Name = name;

        public PageExpression(int? defaultValue = null) => DefaultValue = defaultValue;

        public IPageExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        IPageExpression<TRequest, TResult> IPageExpression<TRequest, TResult>.DefaultValue(int defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        IPageExpression IPageExpression.DefaultValue(int defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public string Name { get; }

        public PropertyInfo RequestProperty { get; }

        public PropertyInfo ResultProperty { get; private set; }

        public int? DefaultValue { get; private set; }
    }
}