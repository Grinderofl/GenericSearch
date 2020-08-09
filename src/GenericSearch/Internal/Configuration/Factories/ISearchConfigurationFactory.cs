using System.Reflection;
using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface ISearchConfigurationFactory
    {
        SearchConfiguration Create(PropertyInfo requestProperty, IListDefinition source);
    }
}