namespace GenericSearch.Configuration
{
    public interface IPostRedirectGetConfiguration
    {
        string ActionName { get; }
        bool Enabled { get; }
    }
}