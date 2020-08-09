using System.Reflection;

namespace GenericSearch.Internal.Definition
{
    public interface IRowsDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        string Name { get; }
        int? DefaultValue { get; }
    }
}