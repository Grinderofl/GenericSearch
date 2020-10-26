#pragma warning disable 1591
using System.Linq;

namespace GenericSearch
{
    /// <summary>
    /// Provides an interface for GenericSearch search property types.
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// Specifies whether the search should be applied to a provided query.
        /// </summary>
        /// <returns></returns>
        bool IsActive();

        /// <summary>
        /// Applies the search to the provided query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> ApplyToQuery<T>(IQueryable<T> query);
    }
}