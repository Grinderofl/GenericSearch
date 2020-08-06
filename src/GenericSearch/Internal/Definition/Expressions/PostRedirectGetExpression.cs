namespace GenericSearch.Internal.Definition.Expressions
{
    public class PostRedirectGetExpression : IPostRedirectGetExpression, IPostRedirectGetDefinition
    {
        public string ActionName { get; private set; }
        public bool? Enabled { get; private set; }

        public IPostRedirectGetExpression UseActionName(string actionName)
        {
            ActionName = actionName;
            return this;
        }

        public IPostRedirectGetExpression Enable()
        {
            Enabled = true;
            return this;
        }

        public IPostRedirectGetExpression Disable()
        {
            Enabled = false;
            return this;
        }
    }
}