namespace GenericSearch
{
    public interface IPostRedirectGetExpression
    {
        /// <summary>
        /// Configures the name of the action which triggers the action filter.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        IPostRedirectGetExpression UseActionName(string actionName);

        /// <summary>
        /// Configures the action filter trigger to be enabled.
        /// </summary>
        /// <returns></returns>
        IPostRedirectGetExpression Enable();

        /// <summary>
        /// Configures the action filter trigger to be disabled.
        /// </summary>
        /// <returns></returns>
        IPostRedirectGetExpression Disable();
    }
}