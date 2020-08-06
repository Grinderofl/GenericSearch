using GenericSearch.Internal.Definition;
using Microsoft.Extensions.Options;

namespace GenericSearch.Internal.Configuration.Factories
{
    public class TransferValuesConfigurationFactory : ITransferValuesConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public TransferValuesConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public TransferValuesConfiguration Create(IListDefinition definition) =>
            new TransferValuesConfiguration(definition.TransferValuesDefinition?.ActionName ?? options.ListActionName,
                                            definition.TransferValuesDefinition?.Enabled ?? options.TransferValuesEnabled);
    }
}