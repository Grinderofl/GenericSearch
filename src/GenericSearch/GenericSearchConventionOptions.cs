using GenericSearch.Internal;

namespace GenericSearch
{
    /// <summary>
    /// Provides options for GenericSearch Conventions
    /// </summary>
    public class GenericSearchConventionOptions
    {
        /// <summary>
        /// Specifies the property name to use for Sort Order when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordx'
        /// </remarks>
        /// </summary>
        public string DefaultSortOrderPropertyName { get; set; } = ConfigurationConstants.DefaultSortOrderPropertyName;

        /// <summary>
        /// Specifies the property name to use for Sort Direction when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordd'
        /// </remarks>
        /// </summary>
        public string DefaultSortDirectionPropertyName { get; set; } = ConfigurationConstants.DefaultSortDirectionPropertyName;

        /// <summary>
        /// Specifies the property name to use for Page number when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Page'
        /// </remarks>
        /// </summary>
        public string DefaultPageNumberPropertyName { get; set; } = ConfigurationConstants.DefaultPageNumberPropertyName;

        /// <summary>
        /// Specifies the property name to use for Rows when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Rows'
        /// </remarks>
        /// </summary>
        public string DefaultRowsPropertyName { get; set; } = ConfigurationConstants.DefaultRowsPropertyName;
    }
}