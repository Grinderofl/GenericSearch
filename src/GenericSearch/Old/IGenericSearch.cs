using System.Linq;

namespace GenericSearch
{
    /// <summary>
    /// Core GenericSearch interface
    /// </summary>
    public interface IGenericSearch
    {
        /// <summary>
        /// Filters the <paramref name="query"/> results using the provided request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IQueryable<T> Search<T>(IQueryable<T> query, object request);

        /// <summary>
        /// Filters the <paramref name="query"/> results using a cached request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Search<T>(IQueryable<T> query);

        /// <summary>
        /// Sorts the <paramref name="query"/> results using the provided request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IQueryable<T> Sort<T>(IQueryable<T> query, object request);

        /// <summary>
        /// Sorts the <paramref name="query"/> results using a cached request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Sort<T>(IQueryable<T> query);

        /// <summary>
        /// Pages the <paramref name="query"/> results using the provided request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IQueryable<T> Paginate<T>(IQueryable<T> query, object request);

        /// <summary>
        /// Pages the <paramref name="query"/> results using a cached request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Paginate<T>(IQueryable<T> query);
    }
}