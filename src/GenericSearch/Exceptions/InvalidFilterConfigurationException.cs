using System;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents errors in filter configurations
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvalidFilterConfigurationException : Exception
    {
        /// <inheritdoc />
        public InvalidFilterConfigurationException()
        {
        }

        /// <inheritdoc />
        public InvalidFilterConfigurationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public InvalidFilterConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}