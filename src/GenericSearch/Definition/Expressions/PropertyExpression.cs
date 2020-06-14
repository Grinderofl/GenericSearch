using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Definition.Expressions
{
    public class PropertyExpression<TRequest, T, TResult> : IPropertyExpression<TRequest, T, TResult>, IPropertyDefinition
    {
        public PropertyExpression(Expression<Func<TRequest, T>> requestProperty, object defaultValue = null)
        {
            RequestProperty = requestProperty.GetPropertyInfo();
            DefaultValue = defaultValue ?? RequestProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        }

        IPropertyExpression<TRequest, T, TResult> IPropertyExpression<TRequest, T, TResult>.MapTo(Expression<Func<TResult, T>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        IPropertyExpression<TRequest, T, TResult> IPropertyExpression<TRequest, T, TResult>.DefaultValue(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        IPropertyExpression<TRequest, T, TResult> IPropertyExpression<TRequest, T, TResult>.Ignore(bool ignore)
        {
            Ignore = ignore;
            return this;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; private set; }
        public object DefaultValue { get; private set; }
        public bool? Ignore { get; private set; }
    }
}