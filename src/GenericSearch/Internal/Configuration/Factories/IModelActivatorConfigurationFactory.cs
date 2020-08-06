using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IModelActivatorConfigurationFactory
    {
        IModelActivatorConfiguration Create(IListDefinition source);
    }
}