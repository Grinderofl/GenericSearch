using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface ISortColumnConfigurationFactory
    {
        SortColumnConfiguration Create(IListDefinition source);
    }
}