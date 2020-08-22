#pragma warning disable 1591
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GenericSearch
{
    /// <summary>
    /// Provides a base class for searching over the given generic type.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class Search<TEntity> : ISearch
    {
        public abstract bool IsActive();

        /// <inheritdoc />
        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            return ApplyToQuery(query.Cast<TEntity>()).Cast<T>();
        }

        /// <summary>
        /// Applies the search to the provided query over <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract IQueryable<TEntity> ApplyToQuery(IQueryable<TEntity> query);
    }
}