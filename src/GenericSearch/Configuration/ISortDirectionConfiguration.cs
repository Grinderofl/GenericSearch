using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Configuration
{
    public interface ISortDirectionConfiguration
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        string Name { get; }
        Direction DefaultValue { get; }
    }
}