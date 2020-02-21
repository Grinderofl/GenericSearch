#pragma warning disable 1591
using Grinderofl.GenericSearch.Searches;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ISearchExpression
    {
        ISearch Search { get; }
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
    }
}