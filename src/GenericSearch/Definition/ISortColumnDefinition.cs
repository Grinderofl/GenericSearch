using System.Reflection;

namespace GenericSearch.Definition
{
    public interface ISortColumnDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        PropertyInfo DefaultProperty { get; }
        string Name { get; }
        object DefaultValue { get; }
    }
}