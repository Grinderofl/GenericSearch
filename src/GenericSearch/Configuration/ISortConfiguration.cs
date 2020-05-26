using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides configuration for sorting specification
    /// </summary>
    public interface ISortConfiguration
    {
        /// <summary>
        /// Specifies the request sort property info
        /// </summary>
        PropertyInfo RequestSortProperty { get; }

        /// <summary>
        /// Specifies the result sort property info
        /// </summary>
        PropertyInfo ResultSortProperty { get; }

        /// <summary>
        /// Specifies the request sort direction info
        /// </summary>
        PropertyInfo RequestSortDirection { get; }

        /// <summary>
        /// Specifies the result sort direction info
        /// </summary>
        PropertyInfo ResultSortDirection { get; }

        /// <summary>
        /// Specifies the default sort direction
        /// </summary>
        Direction? DefaultSortDirection { get; }
    }
}