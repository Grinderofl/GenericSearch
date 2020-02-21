#pragma warning disable 1591
using Grinderofl.GenericSearch.Searches;
using Grinderofl.GenericSearch.Transformers;
using Microsoft.AspNetCore.Routing;

namespace Grinderofl.GenericSearch.Configuration
{
    public class ConventionOptions
    {
        /// <summary>
        /// Gets or sets the default name convention for Page number properties.
        /// </summary>
        public string PagePropertyName { get; set; } = GenericSearchConstants.PagePropertyName;

        /// <summary>
        /// Gets or sets the default value convention for page number properties.
        /// </summary>
        public int DefaultPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the default name convention for Rows properties.
        /// </summary>
        public string RowsPropertyName { get; set; } = GenericSearchConstants.RowsPropertyName;

        /// <summary>
        /// Gets or sets the default value convention for Rows properties.
        /// </summary>
        public int DefaultRows { get; set; } = 25;

        /// <summary>
        /// Gets or sets the default name convention for SortBy properties.
        /// </summary>
        public string SortByPropertyName { get; set; } = GenericSearchConstants.SortByPropertyName;

        /// <summary>
        /// Gets or sets the default name convention for SortDirection properties.
        /// </summary>
        public string SortDirectionPropertyName { get; set; } = GenericSearchConstants.SortDirectionPropertyName;

        /// <summary>
        /// Gets or sets the default value convention for SortDirection properties.
        /// </summary>
        public Direction DefaultSortDirection { get; set; } = Direction.Ascending;

        /// <summary>
        /// Gets or sets whether <see cref="SearchProfile{TEntity,TRequest,TResult}"/> instances should be initialised with convention fallback.
        /// <para>
        ///     When true, all TRequest and TResult types will have their <see cref="AbstractSearch"/> property types automatically initialised<br/>
        ///     with their respective concrete types using the property of TEntity with the same name as the TRequest property, the sort and paging properties<br/>
        ///     initialised where they exist, the properties transferred from TRequest to TResult, and POST requests to Index actions redirected to same action<br/>
        ///     using a <see cref="IRouteValueTransformer"/> to convert the TRequest action parameter to a <see cref="RouteValueDictionary"/> and assign it to<br/>
        ///     the routeValues argument.
        /// </para>
        /// </summary>
        public bool UseConventions { get; set; }
    }
}