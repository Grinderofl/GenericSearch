using System.Reflection;

namespace GenericSearch.Configuration.Internal
{
    /// <summary>
    /// Provides configuration for page specification
    /// </summary>
    public class PageConfiguration : IPageConfiguration
    {
        /// <summary>
        /// Specifies the request property to use for page number
        /// </summary>
        public PropertyInfo RequestPageNumberProperty { get; set; }

        /// <summary>
        /// Specifies the request property to use for rows
        /// </summary>
        public PropertyInfo RequestRowsProperty { get; set; }

        /// <summary>
        /// Specifies the result property to use for page number
        /// </summary>
        public PropertyInfo ResultPageNumberProperty { get; set; }

        /// <summary>
        /// Specifies the result property to use for rows
        /// </summary>
        public PropertyInfo ResultRowsProperty { get; set; }

        /// <summary>
        /// Specifies the default rows per page
        /// </summary>
        public int DefaultRowsPerPage { get; set; }

        /// <summary>
        /// Specifies the default page number
        /// </summary>
        public int DefaultPageNumber { get; set; }
    }
}