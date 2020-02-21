using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Linq;

namespace Grinderofl.GenericSearch.Extensions
{
    internal static class ActionDescriptorExtensions
    {
        public static ParameterDescriptor GetParameterDescriptor(this ActionDescriptor descriptor, Type parameterType)
        {
            return descriptor.Parameters.First(x => x.ParameterType == parameterType);
        }
    }
}