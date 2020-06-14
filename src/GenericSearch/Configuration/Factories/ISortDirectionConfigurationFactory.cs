using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface ISortDirectionConfigurationFactory
    {
        SortDirectionConfiguration Create(IListDefinition source);
    }
}