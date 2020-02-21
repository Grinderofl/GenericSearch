#pragma warning disable 1591
using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ISearchExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the search property on TRequest
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TResult> WithRequestProperty(Expression<Func<TRequest, object>> expression);

        /// <summary>
        /// Specifies the search property on TResult
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TResult> WithResultProperty(Expression<Func<TResult, object>> expression);
    }
}