using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IRowsConfigurationFactory
    {
        RowsConfiguration Create(IListDefinition source);
    }
}