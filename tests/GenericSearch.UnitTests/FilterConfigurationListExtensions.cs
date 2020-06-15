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

        public static PropertyInfo ItemPropertyFor(this ListConfiguration configuration, string requestPropertyName)
        {
            return configuration.SearchConfigurations.ItemPropertyFor(requestPropertyName);
        }

        
        public static SearchConfiguration Find(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.First(x => x.RequestProperty.Name == requestPropertyName);
        }

        public static PropertyInfo ResultPropertyFor(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ResultProperty;
        }

        public static PropertyInfo ItemPropertyFor(this IEnumerable<SearchConfiguration> list, string requestPropertyName)
        {
            return list.Find(requestPropertyName).ItemProperty;
        }
    }
}