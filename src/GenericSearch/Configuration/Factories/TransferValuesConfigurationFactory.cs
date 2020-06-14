using GenericSearch.Definition;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
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