using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IPostRedirectGetConfigurationFactory
    {
        PostRedirectGetConfiguration Create(IListDefinition source);
    }
}