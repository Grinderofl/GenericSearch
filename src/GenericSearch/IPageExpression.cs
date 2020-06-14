using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IPageExpression
    {
        IPageExpression DefaultValue(int defaultValue);
    }

    public interface IPageExpression<TRequest, TResult>
    {
        IPageExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property);
        IPageExpression<TRequest, TResult> DefaultValue(int defaultValue);
    }
}