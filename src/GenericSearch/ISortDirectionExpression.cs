using System;
using System.Linq.Expressions;
using GenericSearch.Searches;

namespace GenericSearch
{
    public interface ISortDirectionExpression<TRequest, TResult>
    {
        /// <summary>
        /// Configures the sort direction request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        ISortDirectionExpression<TRequest, TResult> MapTo(Expression<Func<TResult, Direction>> property);

        /// <summary>
        /// Configures the default sort direction.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        ISortDirectionExpression<TRequest, TResult> DefaultValue(Direction defaultValue);
    }
}