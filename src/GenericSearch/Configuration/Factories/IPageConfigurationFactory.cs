using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IPageConfigurationFactory
    {
        PageConfiguration Create(IListDefinition source);
    }
}