using System.Reflection;

namespace GenericSearch.Definition
{
    public interface IRowsDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        string Name { get; }
        int? DefaultValue { get; }
    }
}