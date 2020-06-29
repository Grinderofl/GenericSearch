using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IPageExpression
    {
        /// <summary>
        /// Configures the default page number.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IPageExpression DefaultValue(int defaultValue);
    }

    public interface IPageExpression<TRequest, TResult>
    {
        /// <summary>
        /// Configures the page number request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IPageExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property);

        /// <summary>
        /// Configures the default page number.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IPageExpression<TRequest, TResult> DefaultValue(int defaultValue);
    }
}