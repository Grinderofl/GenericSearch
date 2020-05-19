using System;
using System.Linq.Expressions;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Provides additional configuration for specific properties on <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface ISearchExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the property on <typeparamref name="TResult"/> which the of <typeparamref name="TRequest"/>
        /// should be transferred to after an action has been executed.
        /// </summary>
        /// <param name="propertyExpression">Result/ViewModel property to use for transfer</param>
        /// <returns>Criterion expression</returns>
        ISearchExpression<TRequest, TResult> MapTo(Expression<Func<TResult, object>> propertyExpression);

        /// <summary>
        /// Specifies that the search property on <typeparamref name="TRequest"/> should be ignored while performing
        /// search, e.g. in situations where the search property is used in a different way.
        /// </summary>
        /// <returns>Criterion expression</returns>
        ISearchExpression<TRequest, TResult> Ignore();

        /// <summary>
        /// Specifies that the search property on <typeparamref name="TRequest"/> should use a custom method
        /// to initialize the search property.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns>Criterion expression</returns>
        ISearchExpression<TRequest, TResult> UseSearch(Func<ISearch> factory);

    }
}