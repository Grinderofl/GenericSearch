using GenericSearch.Definition;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
{
    public class PostRedirectGetConfigurationFactory : IPostRedirectGetConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public PostRedirectGetConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public PostRedirectGetConfiguration Create(IListDefinition definition) =>
            new PostRedirectGetConfiguration(definition.PostRedirectGetDefinition?.ActionName ?? options.ListActionName,
                                             definition.PostRedirectGetDefinition?.Enabled ?? options.PostRedirectGetEnabled);
    }
}