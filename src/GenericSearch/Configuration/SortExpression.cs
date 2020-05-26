using System;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides sorting specification expression
    /// </summary>
    /// <typeparam name="TItem">Entity/Projection type</typeparam>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class SortExpression<TItem, TRequest, TResult> : ISortConfiguration,
                                                            ISortExpression<TItem, TRequest, TResult>
    {
        /// <summary>
        /// Specifies the request sort property info
        /// </summary>
        public PropertyInfo RequestSortProperty { get; private set; }

        /// <summary>
        /// Specifies the result sort property info
        /// </summary>
        public PropertyInfo ResultSortProperty { get; private set; }

        /// <summary>
        /// Specifies the request sort direction info
        /// </summary>
        public PropertyInfo RequestSortDirection { get; private set; }

        /// <summary>
        /// Specifies the result sort direction info
        /// </summary>
        public PropertyInfo ResultSortDirection { get; private set; }

        /// <summary>
        /// Specifies the default sort direction
        /// </summary>
        public Direction? DefaultSortDirection { get; private set; }
        
        /// <summary>
        /// Specifies the property to use for sort property
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Sort expression</returns>
        public ISortExpression<TItem, TRequest, TResult> Property(Expression<Func<TRequest, object>> requestProperty, 
                                                                  Expression<Func<TResult, object>> resultProperty = null)
        {
            RequestSortProperty = requestProperty.GetPropertyInfo();
            ResultSortProperty = resultProperty?.GetPropertyInfo();
            return this;
        }
        /// <summary>
        /// Specifies the property to use for sort direction
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Sort expression</returns>
        public ISortExpression<TItem, TRequest, TResult> Direction(Expression<Func<TRequest, object>> requestProperty, Expression<Func<TResult, object>> resultProperty = null)
        {
            RequestSortDirection = requestProperty.GetPropertyInfo();
            ResultSortDirection = resultProperty?.GetPropertyInfo();
            return this;
        }

        /// <summary>
        /// Specifies the default sort direction
        /// </summary>
        /// <param name="defaultDirection">Default sort direction</param>
        /// <returns>Sort expression</returns>
        public ISortExpression<TItem, TRequest, TResult> DefaultDirection(Direction defaultDirection)
        {
            DefaultSortDirection = defaultDirection;
            return this;
        }
    }
}