#pragma warning disable 1591
using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ISortExpression<TEntity, TRequest, TResult>
    {
        /// <summary>
        /// Specifies the Sort By Property on TRequest
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithRequestSortByProperty(Expression<Func<TRequest, object>> expression);

        /// <summary>
        /// Specifies the Sort Direction Property on TRequest
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithRequestSortDirectionProperty(Expression<Func<TRequest, object>> expression);

        /// <summary>
        /// Specifies the Sort By Property on TRequest
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithResultSortByProperty(Expression<Func<TResult, object>> expression);

        /// <summary>
        /// Specifies the Sort Direction Property on TResult
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithResultSortDirectionProperty(Expression<Func<TResult, object>> expression);

        /// <summary>
        /// Specifies the default Sort Direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithDefaultSortDirection(Direction direction);

        /// <summary>
        /// Specifies the default Sort By on TEntity
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISortExpression<TEntity, TRequest, TResult> WithDefaultSortBy(Expression<Func<TEntity, object>> expression);
    }
}