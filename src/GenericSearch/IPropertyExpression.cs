using System;
using System.Linq.Expressions;

namespace GenericSearch
{
    public interface IPropertyExpression<TRequest, T, TResult>
    {
        /// <summary>
        /// Configures the request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IPropertyExpression<TRequest, T, TResult> MapTo(Expression<Func<TResult, T>> property);

        /// <summary>
        /// Configures the default value for the request property.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IPropertyExpression<TRequest, T, TResult> DefaultValue(object defaultValue = null);

        /// <summary>
        /// Configures the request property to be ignored by model binder and action filters.
        /// </summary>
        /// <param name="ignore"></param>
        /// <returns></returns>
        IPropertyExpression<TRequest, T, TResult> Ignore(bool ignore = true);
    }
}