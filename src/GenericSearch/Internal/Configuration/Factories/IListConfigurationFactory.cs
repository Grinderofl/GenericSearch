using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IListConfigurationFactory
    {
        IListConfiguration Create(IListDefinition source);
    }
}