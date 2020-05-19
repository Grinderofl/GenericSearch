using System;
using System.Linq.Expressions;
using System.Reflection;
using Grinderofl.GenericSearch.Internal.Extensions;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides additional configuration for specific properties on <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class SearchExpression<TRequest, TResult> : ISearchConfiguration, 
                                                       ISearchExpression<TRequest, TResult>
    {
        /// <summary>
        /// Initialises a new instance of CriterionExpression.
        /// </summary>
        public SearchExpression()
        {
        }


        /// <summary>
        /// Initialises a new instance of CriterionExpression using a factory method.
        /// </summary>
        /// <param name="factory">Factory method for initialising the search property</param>
        /// <param name="requestProperty">Request/Parameter property</param>
        public SearchExpression(Func<ISearch> factory, PropertyInfo requestProperty)
        {
            SearchFactory = factory;
            RequestProperty = requestProperty;
        }


        /// <summary>
        /// Specifies whether the property should be ignored during search
        /// </summary>
        public bool IsIgnored { get; private set; }

        /// <summary>
        /// Specifies the search property on request/parameter type
        /// </summary>
        public PropertyInfo RequestProperty { get; }

        /// <summary>
        /// Specifies the property on result/viewmodel which the value of search property
        /// should be transferred to
        /// </summary>
        public PropertyInfo ResultProperty { get; private set; }

        /// <summary>
        /// Specifies the method to use when initialising the search property
        /// </summary>
        public Func<ISearch> SearchFactory { get; private set; }

        /// <summary>
        /// Specifies the property on <typeparamref name="TResult"/> which the of <typeparamref name="TRequest"/>
        /// should be transferred to after an action has been executed.
        /// </summary>
        /// <param name="propertyExpression">Result/ViewModel property to use for transfer</param>
        /// <returns>Criterion expression</returns>
        public ISearchExpression<TRequest, TResult> MapTo(Expression<Func<TResult, object>> propertyExpression)
        {
            ResultProperty = propertyExpression.GetPropertyInfo();
            return this;
        }

        /// <summary>
        /// Specifies that the search property on <typeparamref name="TRequest"/> should be ignored while performing
        /// search, e.g. in situations where the search property is used in a different way.
        /// </summary>
        /// <returns>Criterion expression</returns>
        public ISearchExpression<TRequest, TResult> Ignore()
        {
            IsIgnored = true;
            return this;
        }

        /// <summary>
        /// Specifies that the search property on <typeparamref name="TRequest"/> should use a custom method
        /// to initialize the search property.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns>Criterion expression</returns>
        public ISearchExpression<TRequest, TResult> UseSearch(Func<ISearch> factory)
        {
            SearchFactory = factory;
            return this;
        }
    }
}