namespace GenericSearch
{
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