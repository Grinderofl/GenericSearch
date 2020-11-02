using System.ComponentModel;
using System.Linq;
using GenericSearch.Internal.Configuration;

namespace GenericSearch
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IGenericSearch
    {
        IQueryable<T> Search<T>(IQueryable<T> query, IListConfiguration configuration, object request);

        IQueryable<T> Search<T>(IQueryable<T> query, object request);

        IQueryable<T> Search<T>(IQueryable<T> query);

        IQueryable<T> Sort<T>(IQueryable<T> query, IListConfiguration configuration, object request);

        IQueryable<T> Sort<T>(IQueryable<T> query, object request);

        IQueryable<T> Sort<T>(IQueryable<T> query);

        IQueryable<T> Paginate<T>(IQueryable<T> query, IListConfiguration configuration, object request);

        IQueryable<T> Paginate<T>(IQueryable<T> query, object request);

        IQueryable<T> Paginate<T>(IQueryable<T> query);
    }
}