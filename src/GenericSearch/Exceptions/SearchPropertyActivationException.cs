using System;
using System.Diagnostics.CodeAnalysis;
using GenericSearch.Searches;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the activation of <see cref="ISearch"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SearchPropertyActivationException : Exception
    {
        /// <inheritdoc />
        public SearchPropertyActivationException()
        {
        }

        /// <inheritdoc />
        public SearchPropertyActivationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public SearchPropertyActivationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}