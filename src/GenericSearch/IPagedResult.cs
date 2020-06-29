namespace GenericSearch
{
    public interface IPagedResult
    {
        /// <summary>
        /// Specifies the current page
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Specifies number of rows per page
        /// </summary>
        int Rows { get; set; }

        /// <summary>
        /// Specifies the total number of results
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Specifies the total number of pages
        /// </summary>
        int Pages { get; }

        /// <summary>
        /// Specifies whether there is a previous page
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Specifies whether there is a next page
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Specifies the number of the previous page
        /// </summary>
        int PreviousPage { get; }

        /// <summary>
        /// Specifies the number of the next page
        /// </summary>
        int NextPage { get; }

        /// <summary>
        /// Specifies the start of the page range
        /// </summary>
        int StartPage { get; }

        /// <summary>
        /// Specfies the end of the page range
        /// </summary>
        int EndPage { get; }
    }
}