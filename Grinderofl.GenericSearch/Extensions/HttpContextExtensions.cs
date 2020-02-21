using Microsoft.AspNetCore.Http;
using System;

namespace Grinderofl.GenericSearch.Extensions
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