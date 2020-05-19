using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grinderofl.GenericSearch.Searches;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Grinderofl.GenericSearch.Internal.Extensions
{
    internal static class ModelBindingContextExtensions
    {
        public static IEnumerable<PropertyInfo> FindSearchProperties(this ModelBindingContext modelBindingContext)
        {
            return modelBindingContext.ModelType.GetProperties()
                                      .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(ISearch)));
        }
    }
}