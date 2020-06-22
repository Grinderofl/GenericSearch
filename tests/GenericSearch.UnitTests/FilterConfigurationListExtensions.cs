using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Configuration;

namespace GenericSearch.UnitTests
{
    internal static class ListConfigurationExtensions
    {
        public static PropertyInfo ResultPropertyFor(this ListConfiguration configuration, string requestPropertyName)
        {
            return configuration.SearchConfigurations.ResultPropertyFor(requestPropertyName);
        }

        public static string ItemPropertyPathFor(this ListConfiguration configuration, string requestPropertyName)
        {
            return configuration.SearchConfigurations.ItemPropertyPathFor(requestPropertyName);
        }

        
        public static SearchConfiguration Find(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.First(x => x.RequestProperty.Name == requestPropertyName);
        }

        public static PropertyInfo ResultPropertyFor(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ResultProperty;
        }

        public static string ItemPropertyPathFor(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ItemPropertyPath;
        }
    }
}