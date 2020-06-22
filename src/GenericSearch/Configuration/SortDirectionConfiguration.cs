using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Configuration
{
    public class SortDirectionConfiguration : ISortDirectionConfiguration
    {
        public SortDirectionConfiguration(PropertyInfo requestProperty, PropertyInfo resultProperty, Direction defaultValue)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
            DefaultValue = defaultValue;
        }

        public SortDirectionConfiguration(string name, Direction defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public string Name { get; }
        public Direction DefaultValue { get; }
    }
}