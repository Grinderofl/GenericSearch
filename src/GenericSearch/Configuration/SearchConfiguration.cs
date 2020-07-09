using System;
using System.Diagnostics;
using System.Reflection;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;

namespace GenericSearch.Configuration
{
    [DebuggerDisplay("FilterConfiguration: Request = {RequestProperty.Name} Item = {ItemPropertyPath} Result = {ResultProperty.Name} Ignored = {Ignored}")]
    public class SearchConfiguration : ISearchConfiguration
    {
        public SearchConfiguration(PropertyInfo requestProperty)
        {
            RequestProperty = requestProperty;
        }

        public bool Ignored { get; set; }
        public PropertyInfo RequestProperty { get; }

        public string ItemPropertyPath { get; set; }
        public PropertyInfo ResultProperty { get; set; }
        public Func<ISearch> Constructor { get; set; }
        public Func<IServiceProvider, ISearchActivator> Activator { get; set; }
    }
}