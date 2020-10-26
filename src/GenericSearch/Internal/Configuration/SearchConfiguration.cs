using System;
using System.Diagnostics;
using System.Reflection;

namespace GenericSearch.Internal.Configuration
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

        public string[] ItemPropertyPath { get; set; }
        public PropertyInfo ResultProperty { get; set; }
        public Func<ISearch> Constructor { get; set; }
        public Func<IServiceProvider, ISearchActivator> Activator { get; set; }
    }
}