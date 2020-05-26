using GenericSearch.Configuration;
using GenericSearch.Searches;

namespace GenericSearch.Internal.Extensions
{
    internal static class SortConfigurationExtensions
    {
        public static Direction? GetDefaultSortDirection(this ISortConfiguration configuration)
        {
            if (configuration.DefaultSortDirection != null)
            {
                return configuration.DefaultSortDirection;
            }

            var defaultValue = configuration.RequestSortDirection.GetDefaultValue<Direction?>();
            return defaultValue;
        }
    }
}