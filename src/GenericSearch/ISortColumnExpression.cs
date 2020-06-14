using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface ISortColumnExpression<TRequest, TItem, TResult>
    {
        ISortColumnExpression<TRequest, TItem, TResult> MapTo(Expression<Func<TResult, string>> property);
        ISortColumnExpression<TRequest, TItem, TResult> DefaultTo(Expression<Func<TItem, object>> defaultValue);
        ISortColumnExpression<TRequest, TItem, TResult> DefaultValue(object defaultValue);
    }
}