namespace GenericSearch.Internal.Definition.Expressions
{
    public class TransferValuesExpression : ITransferValuesExpression, ITransferValuesDefinition
    {
        public string ActionName { get; private set; }
        public bool? Enabled { get; private set; }

        public ITransferValuesExpression UseActionName(string actionName)
        {
            ActionName = actionName;
            return this;
        }

        public ITransferValuesExpression Enable()
        {
            Enabled = true;
            return this;
        }


        public ITransferValuesExpression Disable()
        {
            Enabled = false;
            return this;
        }
    }
}