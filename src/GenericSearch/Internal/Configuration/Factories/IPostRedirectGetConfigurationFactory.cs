using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface IPostRedirectGetConfigurationFactory
    {
        PostRedirectGetConfiguration Create(IListDefinition source);
    }
}