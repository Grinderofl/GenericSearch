namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides configuration for redirect POST to GET specification
    /// </summary>
    public interface IRedirectPostToGetConfiguration
    {
        /// <summary>
        /// Specifies the name of the action which should trigger POST to GET redirects
        /// </summary>
        string ActionName { get; }

        /// <summary>
        /// Specifies whether POST to GET redirects are enabled, disabled, or what is specified in options
        /// </summary>
        ConfigurationState ConfigurationState { get; }
    }
}