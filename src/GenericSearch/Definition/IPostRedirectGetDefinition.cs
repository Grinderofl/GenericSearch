namespace GenericSearch.Definition
{
    public interface IPostRedirectGetDefinition
    {
        string ActionName { get; }
        bool? Enabled { get; }
    }
}