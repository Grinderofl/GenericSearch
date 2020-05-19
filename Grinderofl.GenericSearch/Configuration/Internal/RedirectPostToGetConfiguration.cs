namespace Grinderofl.GenericSearch.Configuration.Internal
{
    /// <summary>
    /// Provides configuration for redirect POST to GET specification
    /// </summary>
    public class RedirectPostToGetConfiguration : IRedirectPostToGetConfiguration
    {
        /// <summary>
        /// Specifies the name of the action which should trigger POST to GET redirects
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Specifies whether POST to GET redirects are enabled, disabled, or what is specified in options
        /// </summary>
        public ConfigurationState ConfigurationState { get; set; }
    }
}