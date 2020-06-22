using System.Reflection;

namespace GenericSearch.Configuration
{
    public interface ISortColumnConfiguration
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        string Name { get; }
        object DefaultValue { get; }
    }
}