using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IRowsConfigurationFactory
    {
        RowsConfiguration Create(IListDefinition source);
    }
}