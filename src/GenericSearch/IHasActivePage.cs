namespace GenericSearch
{
    /// <summary>
    /// Provides an interface for a view model which determines whether given page number is the same as the currently active page.
    /// </summary>
    public interface IHasActivePage
    {
        /// <summary>
        /// Specifies whether the provided page number is the currently active page
        /// </summary>
        /// <param name="page">Page number to check</param>
        /// <returns></returns>
        bool IsActivePage(int page);
    }
}