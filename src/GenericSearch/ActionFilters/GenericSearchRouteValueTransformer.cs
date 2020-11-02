using System;
using GenericSearch.ActionFilters.Visitors;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters
{
    public class GenericSearchRouteValueTransformer : IGenericSearchRouteValueTransformer
    {
        public RouteValueDictionary Transform(object model, Type requestType)
        {
            var rvd = new RouteValueDictionary();
            var visitor = new ModelPropertyVisitor(model, rvd);
            foreach (var property in requestType.GetProperties())
            {
                visitor.Visit(property);
            }

            return rvd;
        }
    }
}