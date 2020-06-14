using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Definition.Expressions
{
    public class RowsExpression<TRequest, TResult> : IRowsDefinition, IRowsExpression<TRequest, TResult>, IRowsExpression
    {
        public RowsExpression(Expression<Func<TRequest, int>> property)
        {
            RequestProperty = property.GetPropertyInfo();
            DefaultValue = (int?) RequestProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        }

        public RowsExpression(string name = null)
        {
            Name = name;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; private set; }
        public string Name { get; }
        public int? DefaultValue { get; private set; }

        public IRowsExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        IRowsExpression<TRequest, TResult> IRowsExpression<TRequest, TResult>.DefaultValue(int? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        IRowsExpression IRowsExpression.DefaultValue(int? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
    }
}