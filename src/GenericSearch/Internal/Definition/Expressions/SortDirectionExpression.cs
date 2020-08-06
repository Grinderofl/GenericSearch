using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;

namespace GenericSearch.Internal.Definition.Expressions
{
    public class SortDirectionExpression<TRequest, TResult> : ISortDirectionExpression<TRequest, TResult>, ISortDirectionDefinition
    {
        public SortDirectionExpression(Expression<Func<TRequest, Direction>> requestProperty)
        {
            RequestProperty = requestProperty.GetPropertyInfo();
            DefaultValue = (Direction?) RequestProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        }

        public SortDirectionExpression(string name = null) => Name = !string.IsNullOrWhiteSpace(name) ? name : null;

        public PropertyInfo RequestProperty { get; set; }
        public PropertyInfo ResultProperty { get; set; }
        public string Name { get; set; }
        public Direction? DefaultValue { get; set; }

        public ISortDirectionExpression<TRequest, TResult> MapTo(Expression<Func<TResult, Direction>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        ISortDirectionExpression<TRequest, TResult> ISortDirectionExpression<TRequest, TResult>.DefaultValue(Direction defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
    }
}