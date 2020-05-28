using System;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents error when a configuration is missing
    /// </summary>
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