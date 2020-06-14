using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Configuration.Internal
{
    /// <summary>
    /// Provides configuration for sorting specification
    /// </summary>
    public class SortConfiguration : ISortConfiguration
    {
        /// <summary>
        /// Specifies the request sort property info
        /// </summary>
        public PropertyInfo RequestSortProperty { get; set; }

        /// <summary>
        /// Specifies the result sort property info
        /// </summary>
        public PropertyInfo ResultSortProperty { get; set; }

        /// <summary>
        /// Specifies the request sort direction info
        /// </summary>
        public PropertyInfo RequestSortDirection { get; set; }

        /// <summary>
        /// Specifies the result sort direction info
        /// </summary>
        public PropertyInfo ResultSortDirection { get; set; }


        /// <summary>
        /// Specifies the default sort direction
        /// </summary>
        public Direction? DefaultSortDirection { get; set; }

        /// <summary>
        /// Specifies the default sort property
        /// </summary>
        public PropertyInfo DefaultSortProperty { get; set; }
    }
}