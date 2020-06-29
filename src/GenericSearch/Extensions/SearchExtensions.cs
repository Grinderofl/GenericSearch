using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GenericSearch.Extensions
{
    /// <summary>
    /// Extension methods for GenericSearch
    /// </summary>
    public static class SearchExtensions
    {
        /// <summary>
        /// Filters the <paramref name="query"/> results using the provided request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> query, IGenericSearch search, object request)
        {
            return search.Search(query, request);
        }

        /// <summary>
        /// Filters the <paramref name="query"/> results using a cached request object to obtain the search parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> query, IGenericSearch search)
        {
            return search.Search(query);
        }

        /// <summary>
        /// Sorts the <paramref name="query"/> results using the provided request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IGenericSearch search, object request)
        {
            return search.Sort(query, request);
        }

        /// <summary>
        /// Sorts the <paramref name="query"/> results using a cached request object to obtain the sort parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IGenericSearch search)
        {
            return search.Sort(query);
        }

        /// <summary>
        /// Pages the <paramref name="query"/> results using the provided request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, IGenericSearch search, object request)
        {
            return search.Paginate(query, request);
        }

        /// <summary>
        /// Pages the <paramref name="query"/> results using a cached request object to obtain the paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, IGenericSearch search)
        {
            return search.Paginate(query);
        }
    }
}