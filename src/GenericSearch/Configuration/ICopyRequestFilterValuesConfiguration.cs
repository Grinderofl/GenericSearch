namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides configuration for copying request filter values to result filter values
    /// </summary>
    public interface ICopyRequestFilterValuesConfiguration
    {
        /// <summary>
        /// Specifies the name of the action which should trigger copying request filter values to result filter values
        /// </summary>
        string ActionName { get; }

        /// <summary>
        /// Specifies whether copying is enabled, disabled, or follow the default configuration
        /// </summary>
        ConfigurationState ConfigurationState { get; }
    }
}