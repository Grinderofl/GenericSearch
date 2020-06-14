using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace GenericSearch.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class HttpContextExtensions
    {
        public static bool IsPostRequest(this HttpContext context)
        {
            return context.GetRequestMethod()
                          .Equals("POST", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetRequestMethod(this HttpContext context)
        {
            return context.Request.Method;
        }
    }
}