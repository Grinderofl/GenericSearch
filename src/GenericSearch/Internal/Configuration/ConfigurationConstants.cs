using GenericSearch.Searches;

namespace GenericSearch.Internal.Configuration
{
    internal static class ConfigurationConstants
    {
        public const string DefaultListActionName = "Index";
        public const string DefaultSortOrderPropertyName = "Ordx";
        public const string DefaultSortDirectionPropertyName = "Ordd";
        public const string DefaultPagePropertyName = "Page";
        public const string DefaultRowsPropertyName = "Rows";
        public const int DefaultPage = 1;
        public const int DefaultRows = 25;
        public const Direction DefaultSortDirection = Direction.Ascending;
    }
}