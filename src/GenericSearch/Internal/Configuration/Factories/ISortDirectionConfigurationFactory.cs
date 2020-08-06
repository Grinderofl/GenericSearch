using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface ISortDirectionConfigurationFactory
    {
        SortDirectionConfiguration Create(IListDefinition source);
    }
}