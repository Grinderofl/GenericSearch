using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IPageConfigurationFactory
    {
        PageConfiguration Create(IListDefinition source);
    }
}