namespace GenericSearch.Configuration.Internal
{
    /// <summary>
    /// Provides configuration for copying request filter values to result filter values
    /// </summary>
    public class CopyRequestFilterValuesConfiguration : ICopyRequestFilterValuesConfiguration
    {
        /// <summary>
        /// Specifies the name of the action which should trigger copying request filter values to result filter values
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Specifies whether copying is enabled, disabled, or follow the default configuration
        /// </summary>
        public ConfigurationState ConfigurationState { get; set; }
    }
}