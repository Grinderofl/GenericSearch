#pragma warning disable 1591
using System;

namespace Grinderofl.GenericSearch.Configuration
{
    public class PropertyNullException : ArgumentException
    {
        public static PropertyNullException Create(Type type, string propertyName, string parameterName)
        {
            return new PropertyNullException($"The type '{type.FullName}' does not contain Property '{propertyName}'.", parameterName);
        }

        public PropertyNullException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}