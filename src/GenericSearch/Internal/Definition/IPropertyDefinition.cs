using System.Reflection;

namespace GenericSearch.Internal.Definition
{
    public interface IPropertyDefinition
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        object DefaultValue { get; }
        bool? Ignore { get; }
    }
}