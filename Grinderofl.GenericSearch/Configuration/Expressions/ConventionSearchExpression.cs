using Grinderofl.GenericSearch.Searches;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class ConventionSearchExpression : ISearchExpression
    {
        public ConventionSearchExpression(ISearch search, PropertyInfo requestProperty, PropertyInfo resultProperty)
        {
            Search = search;
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
        }

        public ISearch Search { get; }
        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
    }
}