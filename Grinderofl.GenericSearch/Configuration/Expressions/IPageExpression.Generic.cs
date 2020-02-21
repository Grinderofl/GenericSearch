#pragma warning disable 1591
using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface IPageExpression<TRequest, TResult>
    {
        IPageExpression<TRequest, TResult> WithRequestPageProperty(Expression<Func<TRequest, object>> expression);
        IPageExpression<TRequest, TResult> WithRequestRowsProperty(Expression<Func<TRequest, object>> expression);
        IPageExpression<TRequest, TResult> WithResultPageProperty(Expression<Func<TResult, object>> expression);
        IPageExpression<TRequest, TResult> WithResultRowsProperty(Expression<Func<TResult, object>> expression);

        IPageExpression<TRequest, TResult> WithDefaultRows(int rows);
        IPageExpression<TRequest, TResult> WithDefaultPage(int page);
    }
}