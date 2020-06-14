using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface ITransferValuesConfigurationFactory
    {
        TransferValuesConfiguration Create(IListDefinition definition);
    }
}