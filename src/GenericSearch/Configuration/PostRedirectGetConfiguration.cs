namespace GenericSearch.Configuration
{
    public class PostRedirectGetConfiguration : IPostRedirectGetConfiguration
    {
        public PostRedirectGetConfiguration(string actionName, bool enabled)
        {
            ActionName = actionName;
            Enabled = enabled;
        }

        public string ActionName { get; }
        public bool Enabled { get; }
    }
}