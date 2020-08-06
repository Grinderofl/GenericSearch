using System.Reflection;
using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IPropertyConfigurationFactory
    {
        PropertyConfiguration Create(PropertyInfo requestProperty, IListDefinition source);
    }
}