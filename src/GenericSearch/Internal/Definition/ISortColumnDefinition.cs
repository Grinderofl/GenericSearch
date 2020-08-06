using System.Reflection;

namespace GenericSearch.Internal.Definition
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