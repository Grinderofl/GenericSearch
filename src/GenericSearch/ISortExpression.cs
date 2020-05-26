using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    /// <summary>
    /// Provides sorting specification expression
    /// </summary>
    /// <typeparam name="TItem">Entity/Projection type</typeparam>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface ISortExpression<TItem, TRequest, TResult>
    {
        /// <summary>
        /// Specifies the property to use for sort property
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Sort expression</returns>
        ISortExpression<TItem, TRequest, TResult> Property(Expression<Func<TRequest, object>> requestProperty,
                                                           Expression<Func<TResult, object>> resultProperty = null);

        /// <summary>
        /// Specifies the property to use for sort direction
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Sort expression</returns>
        ISortExpression<TItem, TRequest, TResult> Direction(Expression<Func<TRequest, object>> requestProperty,
                                                            Expression<Func<TResult, object>> resultProperty = null);

    }
}