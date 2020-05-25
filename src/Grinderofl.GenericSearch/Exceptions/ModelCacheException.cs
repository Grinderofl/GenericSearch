using System;
using Grinderofl.GenericSearch.Configuration.Internal.Caching;

namespace Grinderofl.GenericSearch.Exceptions
{
    /// <summary>
    /// Represents errors in <see cref="ModelCache"/>
    /// </summary>
    public class ModelCacheException : Exception
    {
        /// <inheritdoc />
        public ModelCacheException()
        {
        }

        /// <inheritdoc />
        public ModelCacheException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ModelCacheException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}