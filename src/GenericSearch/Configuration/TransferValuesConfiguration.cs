namespace GenericSearch.Configuration
{
    public class TransferValuesConfiguration : ITransferValuesConfiguration
    {
        public TransferValuesConfiguration(string actionName, bool enabled)
        {
            ActionName = actionName;
            Enabled = enabled;
        }

        public string ActionName { get; }
        public bool Enabled { get; }
    }
}