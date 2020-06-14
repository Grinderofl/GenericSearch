using System.Reflection;

namespace GenericSearch.Configuration
{
    public class PageConfiguration
    {
        public PageConfiguration(PropertyInfo requestProperty, PropertyInfo resultProperty, int defaultValue)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
            DefaultValue = defaultValue;
        }

        public PageConfiguration(string name, int defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public int DefaultValue { get; }

        public string Name { get; }
    }
}