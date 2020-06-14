using System.Reflection;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides configuration for page specification
    /// </summary>
    public interface IPageConfiguration
    {

        /// <summary>
        /// Specifies the request property to use for page number
        /// </summary>
        PropertyInfo RequestPageNumberProperty { get; }
        
        /// <summary>
        /// Specifies the result property to use for page number
        /// </summary>
        PropertyInfo ResultPageNumberProperty { get; }

        /// <summary>
        /// Specifies the request property to use for rows
        /// </summary>
        PropertyInfo RequestRowsProperty { get; }

        /// <summary>
        /// Specifies the result property to use for rows
        /// </summary>
        PropertyInfo ResultRowsProperty { get; }

        /// <summary>
        /// Specifies the default rows per page
        /// </summary>
        int? DefaultRowsPerPage { get; }

        /// <summary>
        /// Specifies the default page number
        /// </summary>
        int? DefaultPageNumber { get; }
    }
}