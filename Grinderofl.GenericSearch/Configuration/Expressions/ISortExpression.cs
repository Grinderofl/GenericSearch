#pragma warning disable 1591
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ISortExpression
    {
        PropertyInfo RequestSortByProperty { get; }
        PropertyInfo RequestSortDirectionProperty { get; }
        PropertyInfo ResultSortByProperty { get; }
        PropertyInfo ResultSortDirectionProperty { get; }

        Direction DefaultSortDirection { get; }
        PropertyInfo DefaultSortBy { get; }
    }
}