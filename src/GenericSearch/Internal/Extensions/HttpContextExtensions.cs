using System;
using Microsoft.AspNetCore.Http;

namespace GenericSearch.Internal.Extensions
{
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