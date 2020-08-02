using System;
using System.Collections.Generic;
using System.Linq;
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
        public GenericSearchOptions()
        {
            DefaultModelActivatorMethod = Activator.CreateInstance;
            DefaultModelActivatorResolver = null;
            DefaultModelActivatorType = null;
        }

        internal List<IListDefinition> Definitions { get; } = new List<IListDefinition>();

        public IListExpression<TRequest, TEntity, TResult> AddList<TRequest, TEntity, TResult>()
        {
            var expression = new ListExpression<TRequest, TEntity, TResult>();
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

        /// <summary>
        /// Specifies the <see cref="IModelFactory"/> to use to activate request models.
        /// <remarks>
        /// The default value is null.
        /// </remarks>
        /// </summary>
        public Type DefaultModelActivatorType { get; set; }

        /// <summary>
        /// Specifies the service provider factory method to use to activate request models.
        /// <remarks>
        /// The default value is null.
        /// </remarks>
        /// </summary>
        public Func<IServiceProvider, Type, object> DefaultModelActivatorResolver { get; set; }

        /// <summary>
        /// Specifies the factory method to use to activate request models.
        /// <remarks>
        /// The default value is <code>Activator.CreateInstance</code>.
        /// </remarks>
        /// </summary>
        public Func<Type, object> DefaultModelActivatorMethod { get; set; }

        public GenericSearchOptions UseModelFactory<T>() where T : class, IModelFactory
        {
            DefaultModelActivatorType = typeof(T);
            return this;
        }

        public GenericSearchOptions UseModelFactory(Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(IModelFactory)))
            {
                throw new ArgumentException($"Provided type does not implement {nameof(IModelFactory)}", nameof(type));
            }

            DefaultModelActivatorType = type;
            return this;
        }

        public GenericSearchOptions UseModelActivator(Func<Type, object> method)
        {
            DefaultModelActivatorMethod = method;
            return this;
        }

        public GenericSearchOptions UseModelActivator(Func<IServiceProvider, Type, object> resolver)
        {
            DefaultModelActivatorResolver = resolver;
            return this;
        }
    }
}