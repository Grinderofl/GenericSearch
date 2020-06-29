using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface ISortColumnExpression<TRequest, TItem, TResult>
    {
        /// <summary>
        /// Configures the sort column request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        ISortColumnExpression<TRequest, TItem, TResult> MapTo(Expression<Func<TResult, string>> property);

        /// <summary>
        /// Configures the default entity property to use for sorting.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        ISortColumnExpression<TRequest, TItem, TResult> DefaultTo(Expression<Func<TItem, object>> defaultValue);

        /// <summary>
        /// Configures the default entity property name to use for sorting.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        ISortColumnExpression<TRequest, TItem, TResult> DefaultValue(object defaultValue);
    }
}