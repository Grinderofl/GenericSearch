using System;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents error when a configuration is missing
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingConfigurationException : Exception
    {
        /// <inheritdoc />
        public MissingConfigurationException()
        {
        }

        /// <inheritdoc />
        public MissingConfigurationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public MissingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}