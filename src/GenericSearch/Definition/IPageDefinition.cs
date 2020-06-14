using System.Reflection;

namespace GenericSearch.Definition
{
    public interface IPageDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        int? DefaultValue { get; }
        string Name { get; }
    }
}