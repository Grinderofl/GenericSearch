using GenericSearch.Configuration;

namespace GenericSearch.Extensions
{
    internal static class FilterConfigurationExtensions
    {
        public static bool IsRedirectPostToGetDisabled(this IFilterConfiguration configuration)
        {
            return configuration.RedirectPostToGetConfiguration.ConfigurationState == ConfigurationState.Disabled;
        }

        public static bool IsCopyRequestFilterValuesDisabled(this IFilterConfiguration configuration)
        {
            return configuration.CopyRequestFilterValuesConfiguration.ConfigurationState == ConfigurationState.Disabled;
        }
    }
}
