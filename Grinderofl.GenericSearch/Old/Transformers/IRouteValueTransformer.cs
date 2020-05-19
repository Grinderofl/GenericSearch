#pragma warning disable 1591
using Microsoft.AspNetCore.Routing;

namespace Grinderofl.GenericSearch.Transformers
{
    public interface IRouteValueTransformer
    {
        /// <summary>
        /// Transforms the provided request object into a <see cref="RouteValueDictionary"/> using the request properties as keys.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RouteValueDictionary Transform(object request);
    }
}