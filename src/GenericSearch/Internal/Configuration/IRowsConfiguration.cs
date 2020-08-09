using System.Reflection;

namespace GenericSearch.Internal.Configuration
{
    public interface IRowsConfiguration
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        string Name { get; }
        int DefaultValue { get; }
    }
}