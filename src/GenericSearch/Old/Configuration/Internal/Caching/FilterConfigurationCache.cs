using System;
using System.Collections.Generic;

namespace GenericSearch.Configuration.Internal.Caching
{
    /// <summary>
    /// Cache for filter configurations
    /// </summary>
    public class FilterConfigurationCache
    {
        private readonly Dictionary<Type, IFilterConfiguration> internalCache = new Dictionary<Type, IFilterConfiguration>();

        /// <summary>
        /// Determines whether the cache contains configuration for the specified type.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Type key) => internalCache.ContainsKey(key);

        /// <summary>
        /// Gets the configuration for the specified type from cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(Type key, out IFilterConfiguration value) => internalCache.TryGetValue(key, out value);

        /// <summary>
        /// Adds the configuration for the specified type to cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(Type key, IFilterConfiguration value) => internalCache.Add(key, value);
    }
}