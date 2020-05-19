namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Provides the state which the configuration setting should follow
    /// </summary>
    public enum ConfigurationState
    {
        /// <summary>
        /// Specifies that the setting to follow the default configuration
        /// </summary>
        Default,

        /// <summary>
        /// Specifies that the setting is enabled
        /// </summary>
        Enabled,

        /// <summary>
        /// Specifies that the setting is disabled
        /// </summary>
        Disabled
    }
}