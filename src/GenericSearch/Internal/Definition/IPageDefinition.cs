using System.Reflection;

namespace GenericSearch.Internal.Definition
{
    public interface IPageDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        int? DefaultValue { get; }
        string Name { get; }
    }
}