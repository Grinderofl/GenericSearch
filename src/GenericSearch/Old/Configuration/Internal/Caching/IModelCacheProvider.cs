namespace GenericSearch.Configuration.Internal.Caching
{
    /// <summary>
    /// </summary>
    public interface IModelCacheProvider
    {
        /// <summary>
        /// Provides a <see cref="ModelCache"/> instance to cache the binded model in
        /// </summary>
        /// <returns></returns>
        ModelCache Provide();
    }
}
