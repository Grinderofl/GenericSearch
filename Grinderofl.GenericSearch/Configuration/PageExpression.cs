using System;
using System.Linq.Expressions;
using System.Reflection;
using Grinderofl.GenericSearch.Internal.Extensions;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides page specification expression
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class PageExpression<TRequest, TResult> : IPageConfiguration,
                                                     IPageExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the request property to use for page number
        /// </summary>
        public PropertyInfo RequestPageNumberProperty { get; private set; }

        /// <summary>
        /// Specifies the result property to use for page number
        /// </summary>
        public PropertyInfo ResultPageNumberProperty { get; private set; }

        /// <summary>
        /// Specifies the request property to use for rows
        /// </summary>
        public PropertyInfo RequestRowsProperty { get; private set; }

        /// <summary>
        /// Specifies the result property to use for rows
        /// </summary>
        public PropertyInfo ResultRowsProperty { get; private set; }

        /// <summary>
        /// Specifies the default rows per page
        /// </summary>
        public int DefaultRowsPerPage { get; private set; }

        /// <summary>
        /// Specifies the default page number
        /// </summary>
        public int DefaultPageNumber { get; private set; }

        /// <summary>
        /// Specifies the property to use for page number
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Page Expression</returns>
        public IPageExpression<TRequest, TResult> Number(Expression<Func<TRequest, object>> requestProperty, Expression<Func<TResult, object>> resultProperty)
        {
            RequestPageNumberProperty = requestProperty.GetPropertyInfo();
            ResultPageNumberProperty = resultProperty?.GetPropertyInfo();
            return this;
        }

        /// <summary>
        /// Specifies the property to use for row count
        /// </summary>
        /// <param name="requestProperty">Property on Request/Parameter</param>
        /// <param name="resultProperty">Property on Result/ViewModel</param>
        /// <returns>Page Expression</returns>
        public IPageExpression<TRequest, TResult> Rows(Expression<Func<TRequest, object>> requestProperty, Expression<Func<TResult, object>> resultProperty)
        {
            RequestRowsProperty = requestProperty.GetPropertyInfo();
            ResultRowsProperty = resultProperty?.GetPropertyInfo();
            return this;
        }


        /// <summary>
        /// Specifies the default number of rows per page
        /// </summary>
        /// <param name="defaultRows">Default rows per page</param>
        /// <returns>Page Expression</returns>
        public IPageExpression<TRequest, TResult> DefaultRows(int defaultRows)
        {
            DefaultRowsPerPage = defaultRows;
            return this;
        }

        /// <summary>
        /// Specifies the default page number
        /// </summary>
        /// <param name="defaultPage">Default page number</param>
        /// <returns>Page Expression</returns>
        public IPageExpression<TRequest, TResult> DefaultPage(int defaultPage)
        {
            DefaultPageNumber = defaultPage;
            return this;
        }
    }
}