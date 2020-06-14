using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IRowsExpression<TRequest, TResult>
    {
        IRowsExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property);
        IRowsExpression<TRequest, TResult> DefaultValue(int? defaultValue);
    }

    public interface IRowsExpression
    {
        IRowsExpression DefaultValue(int? defaultValue);
    }
}