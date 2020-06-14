using System;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents errors which occur when a property is not found
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PropertyNotFoundException : Exception
    {
        /// <inheritdoc />
        public PropertyNotFoundException()
        {
        }

        /// <inheritdoc />
        public PropertyNotFoundException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}