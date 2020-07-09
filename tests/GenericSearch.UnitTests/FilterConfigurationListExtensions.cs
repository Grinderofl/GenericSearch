using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Configuration;

namespace GenericSearch.UnitTests
{
    internal static class ListConfigurationExtensions
    {
        public static PropertyInfo ResultPropertyFor(this IListConfiguration configuration, string requestPropertyName)
        {
            return configuration.SearchConfigurations.ResultPropertyFor(requestPropertyName);
        }

        public static string ItemPropertyPathFor(this IListConfiguration configuration, string requestPropertyName)
        {
            return configuration.SearchConfigurations.ItemPropertyPathFor(requestPropertyName);
        }

        
        public static ISearchConfiguration Find(this IEnumerable<ISearchConfiguration> list, string requestPropertyName)
        {
            return list.First(x => x.RequestProperty.Name == requestPropertyName);
        }

        public static PropertyInfo ResultPropertyFor(this IEnumerable<ISearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ResultProperty;
        }

        public static string ItemPropertyPathFor(this IEnumerable<ISearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ItemPropertyPath;
        }
    }
}