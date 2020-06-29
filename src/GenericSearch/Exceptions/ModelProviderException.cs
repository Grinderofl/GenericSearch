using System;
using System.Diagnostics.CodeAnalysis;
using GenericSearch.Internal;

namespace GenericSearch.Exceptions
{
    /// <summary>
    /// Represents errors in <see cref="RequestModelProvider"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ModelProviderException : Exception
    {
        /// <inheritdoc />
        public ModelProviderException()
        {
        }

        /// <inheritdoc />
        public ModelProviderException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ModelProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}