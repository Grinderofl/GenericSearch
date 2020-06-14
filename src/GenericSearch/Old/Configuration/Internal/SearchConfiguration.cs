using System;
using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Configuration.Internal
{
    internal class SearchConfiguration : ISearchConfiguration
    {
        public SearchConfiguration(PropertyInfo requestProperty)
        {
            RequestProperty = requestProperty;
        }

        public bool IsIgnored { get; set; }
        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; set; }
        public Func<ISearch> SearchFactory { get; set; }
    }
}