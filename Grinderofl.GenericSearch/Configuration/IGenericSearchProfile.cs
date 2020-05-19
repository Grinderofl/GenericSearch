using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Contains filter-specific configuration
    /// </summary>
    public interface IGenericSearchProfile
    {
        /// <summary>
        /// Expressions of filters defined in the profile
        /// </summary>
        IList<IFilterConfiguration> FilterConfigurations { get; }
    }
}