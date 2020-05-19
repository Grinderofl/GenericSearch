using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.Internal
{
    internal static class ConfigurationConstants
    {
        public const string DefaultListActionName = "Index";
        public const string DefaultSortOrderPropertyName = "Ordx";
        public const string DefaultSortDirectionPropertyName = "Ordd";
        public const string DefaultPageNumberPropertyName = "Page";
        public const string DefaultRowsPropertyName = "Rows";
        public const int DefaultPageNumber = 1;
        public const int DefaultRowsPerPage = 25;
        public const Direction DefaultSortDirection = Direction.Ascending;
    }
}