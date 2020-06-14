namespace GenericSearch
{
    public interface IPostRedirectGetExpression
    {
        IPostRedirectGetExpression UseActionName(string actionName);
        IPostRedirectGetExpression Enable();
        IPostRedirectGetExpression Disable();
    }
}