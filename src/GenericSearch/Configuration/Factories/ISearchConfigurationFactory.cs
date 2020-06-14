using System.Reflection;
using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface ISearchConfigurationFactory
    {
        SearchConfiguration Create(PropertyInfo requestProperty, IListDefinition source);
    }
}