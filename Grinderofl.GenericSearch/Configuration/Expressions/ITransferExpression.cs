#pragma warning disable 1591
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ITransferExpression
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
    }
}