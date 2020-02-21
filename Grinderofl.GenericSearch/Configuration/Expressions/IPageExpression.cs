#pragma warning disable 1591
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface IPageExpression
    {
        PropertyInfo RequestPageProperty { get; }
        PropertyInfo RequestRowsProperty { get; }
        PropertyInfo ResultPageProperty { get; }
        PropertyInfo ResultRowsProperty { get; }

        int DefaultRows { get; }
        int DefaultPage { get; }
    }
}