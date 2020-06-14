using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IPropertyExpression<TRequest, T, TResult>
    {
        IPropertyExpression<TRequest, T, TResult> MapTo(Expression<Func<TResult, T>> property);

        IPropertyExpression<TRequest, T, TResult> DefaultValue(object defaultValue = null);
        IPropertyExpression<TRequest, T, TResult> Ignore(bool ignore = true);
    }
}