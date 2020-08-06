namespace GenericSearch.Internal.Definition
{
    public interface IPostRedirectGetDefinition
    {
        string ActionName { get; }
        bool? Enabled { get; }
    }
}