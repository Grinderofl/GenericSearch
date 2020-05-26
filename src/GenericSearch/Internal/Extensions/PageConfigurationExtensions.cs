using GenericSearch.Configuration;

namespace GenericSearch.Internal.Extensions
{
    internal static class PageConfigurationExtensions
    {
        public static int? GetDefaultRowsPerPage(this IPageConfiguration configuration)
        {
            if (configuration.DefaultRowsPerPage > 0)
            {
                return configuration.DefaultRowsPerPage;
            }

            var defaultValue = configuration.RequestRowsProperty.GetDefaultValue<int?>();
            if (defaultValue > 0)
            {
                return defaultValue.Value;
            }

            return null;
        }

        public static int? GetDefaultPageNumber(this IPageConfiguration configuration)
        {
            if (configuration.DefaultPageNumber > 0)
            {
                return configuration.DefaultPageNumber;
            }

            var defaultValue = configuration.RequestPageNumberProperty.GetDefaultValue<int?>();
            if (defaultValue > 0)
            {
                return defaultValue.Value;
            }

            return null;
        }
    }
}