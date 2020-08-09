using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public interface ITransferValuesConfigurationFactory
    {
        TransferValuesConfiguration Create(IListDefinition definition);
    }
}