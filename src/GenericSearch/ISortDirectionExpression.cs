using System;
using System.Linq.Expressions;
using GenericSearch.Searches;

namespace GenericSearch
{
    public interface ISortDirectionExpression<TRequest, TResult>
    {
        ISortDirectionExpression<TRequest, TResult> MapTo(Expression<Func<TResult, Direction>> property);
        ISortDirectionExpression<TRequest, TResult> DefaultValue(Direction defaultValue);
    }
}