using System.Reflection;

namespace GenericSearch.Definition
{
    public interface IPropertyDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        object DefaultValue { get; }
        bool? Ignore { get; }
    }
}