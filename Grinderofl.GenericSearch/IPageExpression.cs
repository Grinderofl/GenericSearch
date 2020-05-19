using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Provides page specification expression
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface IPageExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the property to use for page number
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Page Expression</returns>
        IPageExpression<TRequest, TResult> Number(Expression<Func<TRequest, object>> requestProperty,
                                                  Expression<Func<TResult, object>> resultProperty);

        /// <summary>
        /// Specifies the property to use for row count
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Page Expression</returns>
        IPageExpression<TRequest, TResult> Rows(Expression<Func<TRequest, object>> requestProperty,
                                                Expression<Func<TResult, object>> resultProperty);

        /// <summary>
        /// Specifies the default number of rows per page
        /// </summary>
        /// <param name="defaultRows">Default rows per page</param>
        /// <returns>Page Expression</returns>
        IPageExpression<TRequest, TResult> DefaultRows(int defaultRows);
    }
}