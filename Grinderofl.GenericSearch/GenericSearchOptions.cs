using Grinderofl.GenericSearch.Internal;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Provides options for GenericSearch
    /// </summary>
    public class GenericSearchOptions
    {
        /// <summary>
        /// Specifies whether POST requests should be redirected
        /// to GET globally
        /// <remarks>
        /// Default value is 'true'
        /// </remarks>
        /// </summary>
        public bool RedirectPostToGet { get; set; } = true;

        /// <summary>
        /// Specifies whether filter values should be copied
        /// from request to result globally
        /// <remarks>
        /// Default value is 'true'
        /// </remarks>
        /// </summary>
        public bool CopyRequestFilterValues { get; set; } = true;

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against globally.
        /// <remarks>
        /// Default name is 'Index'
        /// </remarks>
        /// </summary>
        public string DefaultListActionName { get; set; } = ConfigurationConstants.DefaultListActionName;

        /// <summary>
        /// Specifies the default number of rows per page globally
        /// </summary>
        public int DefaultRowsPerPage { get; set; } = ConfigurationConstants.DefaultRowsPerPage;

        /// <summary>
        /// Specifies the default page number globally
        /// </summary>
        public int DefaultPageNumber { get; set; } = ConfigurationConstants.DefaultPageNumber;

        /// <summary>
        /// Specifies the default sort direction globally
        /// </summary>
        public Direction? DefaultSortDirection { get; set; } = ConfigurationConstants.DefaultSortDirection;
    }
}