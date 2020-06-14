using System;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class PropertyTypeMismatchException : Exception
    {
        public PropertyTypeMismatchException()
        {
        }

        public PropertyTypeMismatchException(string message) : base(message)
        {
        }

        public PropertyTypeMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}