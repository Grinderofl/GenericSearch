namespace GenericSearch.Configuration
{
    public class TransferValuesConfiguration
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