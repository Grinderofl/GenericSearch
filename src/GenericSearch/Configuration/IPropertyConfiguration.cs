using System.Reflection;

namespace GenericSearch.Configuration
{
    public interface IPropertyConfiguration
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        object DefaultValue { get; }
        bool Ignored { get; }
    }
}