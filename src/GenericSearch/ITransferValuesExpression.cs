namespace GenericSearch
{
    public interface ITransferValuesExpression
    {
        /// <summary>
        /// Configures the name of the action which triggers the action filter.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        ITransferValuesExpression UseActionName(string actionName);

        /// <summary>
        /// Configures the action filter trigger to be enabled.
        /// </summary>
        /// <returns></returns>
        ITransferValuesExpression Enable();

        /// <summary>
        /// Configures the action filter trigger to be disabled.
        /// </summary>
        /// <returns></returns>
        ITransferValuesExpression Disable();
    }
}