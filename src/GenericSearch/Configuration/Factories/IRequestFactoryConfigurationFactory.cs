using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IRequestFactoryConfigurationFactory
    {
        RequestFactoryConfiguration Create(IListDefinition source);
    }
}