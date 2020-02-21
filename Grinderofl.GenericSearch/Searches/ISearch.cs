#pragma warning disable 1591
using System.Linq;

namespace Grinderofl.GenericSearch.Searches
{
    public interface ISearch
    {
        bool IsActive();
        IQueryable<T> ApplyToQuery<T>(IQueryable<T> query);
    }
}