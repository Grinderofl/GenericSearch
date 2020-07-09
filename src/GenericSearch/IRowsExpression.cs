using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IRowsExpression<TRequest, TResult>
    {
        /// <summary>
        /// Configures the number of rows request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IRowsExpression<TRequest, TResult> MapTo(Expression<Func<TResult, int>> property);

        /// <summary>
        /// Configures the default value to use for the number of rows.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IRowsExpression<TRequest, TResult> DefaultValue(int? defaultValue);
    }

    public interface IRowsExpression
    {
        /// <summary>
        /// Configures the default value to use for the number of rows.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IRowsExpression DefaultValue(int? defaultValue);
    }
}