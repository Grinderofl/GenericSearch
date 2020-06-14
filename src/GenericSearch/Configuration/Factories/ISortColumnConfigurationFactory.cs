using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface ISortColumnConfigurationFactory
    {
        SortColumnConfiguration Create(IListDefinition source);
    }
}