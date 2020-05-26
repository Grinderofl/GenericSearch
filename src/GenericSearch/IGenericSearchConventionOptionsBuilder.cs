using GenericSearch.Configuration;

namespace GenericSearch
{
    /// <summary>
    /// Provides a builder for GenericSearch Convention services
    /// </summary>
    public interface IGenericSearchConventionOptionsBuilder
    {
        /// <summary>
        /// Specifies the property name to use for Sort Order when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordx'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        GenericSearchConventionOptionsBuilder DefaultSortOrderPropertyName(string defaultName);

        /// <summary>
        /// Specifies the property name to use for Sort Direction when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Ordd'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        GenericSearchConventionOptionsBuilder DefaultSortDirectionPropertyName(string defaultName);

        /// <summary>
        /// Specifies the property name to use for Page number when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Page'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        GenericSearchConventionOptionsBuilder DefaultPagePropertyName(string defaultName);

        /// <summary>
        /// Specifies the property name to use for Row count when it hasn't been configured in profile
        /// <remarks>
        /// The default value is 'Rows'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">The default name to use</param>
        GenericSearchConventionOptionsBuilder DefaultRowsPropertyName(string defaultName);
    }
}