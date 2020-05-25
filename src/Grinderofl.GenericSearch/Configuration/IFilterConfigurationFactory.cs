namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides a factory for creating finalized filter configurations
    /// </summary>
    public interface IFilterConfigurationFactory
    {
        /// <summary>
        /// Creates a finalized filter configuration
        /// </summary>
        /// <param name="source">Original filter configuration from <see cref="IGenericSearchProfile"/></param>
        /// <returns>Finalized instance of <see cref="IFilterConfiguration"/></returns>
        IFilterConfiguration Create(IFilterConfiguration source);
    }
}