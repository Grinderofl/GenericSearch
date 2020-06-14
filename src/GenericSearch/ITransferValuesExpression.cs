namespace GenericSearch
{
    public interface ITransferValuesExpression
    {
        ITransferValuesExpression UseActionName(string actionName);
        ITransferValuesExpression Enable();
        ITransferValuesExpression Disable();
    }
}