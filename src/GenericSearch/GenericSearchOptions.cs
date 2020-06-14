using System;
using System.Collections.Generic;
using GenericSearch.Configuration;
using GenericSearch.Definition;
using GenericSearch.Definition.Expressions;
using GenericSearch.Searches;

namespace GenericSearch
{
    /// <summary>
    /// Provides options for GenericSearch
    /// </summary>
    public class GenericSearchOptions
    {
        internal List<IListDefinition> Definitions { get; } = new List<IListDefinition>();

        public IListExpression<TRequest, TItem, TResult> CreateFilter<TRequest, TItem, TResult>()
        {
            var expression = new ListExpression<TRequest, TItem, TResult>();
            Definitions.Add(expression);
            return expression;
        }

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against globally.
        /// <remarks>
        /// Default name is 'Index'
        /// </remarks>
        /// </summary>
        public string ListActionName { get; set; } = ConfigurationConstants.DefaultListActionName;

        /// <summary>
        /// Specifies the default page number globally
        /// <remarks>
        /// The default value is '1'
        /// </remarks>
        /// </summary>
        public int DefaultPage { get; set; } = ConfigurationConstants.DefaultPage;

        /// <summary>
        /// Specifies the default number of rows per page globally
        /// <remarks>
        /// The default value is '25'
        /// </remarks>
        /// </summary>
        public int DefaultRows { get; set; } = ConfigurationConstants.DefaultRows;

        /// <summary>
        /// Specifies the property name to use for Page number when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Page'
        /// </remarks>
        /// </summary>
        public string PagePropertyName { get; set; } = ConfigurationConstants.DefaultPagePropertyName;

        /// <summary>
        /// Specifies the property name to use for Rows when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Rows'
        /// </remarks>
        /// </summary>
        public string RowsPropertyName { get; set; } = ConfigurationConstants.DefaultRowsPropertyName;

        /// <summary>
        /// Specifies the property name to use for Sort Order when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordx'
        /// </remarks>
        /// </summary>
        public string SortColumnPropertyName { get; set; } = ConfigurationConstants.DefaultSortOrderPropertyName;

        /// <summary>
        /// Specifies the property name to use for Sort Direction when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordd'
        /// </remarks>
        /// </summary>
        public string SortDirectionPropertyName { get; set; } = ConfigurationConstants.DefaultSortDirectionPropertyName;

        /// <summary>
        /// Specifies the default sort direction globally
        /// <remarks>
        /// The default value is 'Ascending'
        /// </remarks>
        /// </summary>
        public Direction SortDirection { get; set; } = ConfigurationConstants.DefaultSortDirection;

        /// <summary>
        /// Specifies whether POST requests should be redirected
        /// to GET globally
        /// <remarks>
        /// The default value is 'true'
        /// </remarks>
        /// </summary>
        public bool PostRedirectGetEnabled { get; set; } = true;

        /// <summary>
        /// Specifies whether filter values should be copied
        /// from request to result globally
        /// <remarks>
        /// The default value is 'true'
        /// </remarks>
        /// </summary>
        public bool TransferValuesEnabled { get; set; } = true;

        public Type DefaultRequestFactoryType { get; set; }

        public Func<IServiceProvider, Type, object> DefaultRequestFactoryServiceProvider  { get; set; }

        public Func<Type, object> DefaultRequestFactoryMethod { get; set; } = Activator.CreateInstance;

    }
}