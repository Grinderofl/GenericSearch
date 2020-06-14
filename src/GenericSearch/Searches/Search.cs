#pragma warning disable 1591
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GenericSearch.Searches
{
    [ExcludeFromCodeCoverage]
    public abstract class Search<TEntity> : ISearch
    {
        public abstract bool IsActive();

        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            return ApplyToQuery(query.Cast<TEntity>()).Cast<T>();
        }

        protected abstract IQueryable<TEntity> ApplyToQuery(IQueryable<TEntity> query);
    }
}