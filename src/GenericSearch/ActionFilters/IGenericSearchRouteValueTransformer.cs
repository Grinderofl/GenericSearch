using System;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters
{
    public interface IGenericSearchRouteValueTransformer
    {
        RouteValueDictionary Transform(object model, Type requestType);
    }
}