using System.Reflection;
using GenericSearch.Searches;

namespace GenericSearch.Definition
{
    public interface ISortDirectionDefinition
    {
        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public string Name { get; }
        public Direction? DefaultValue { get; }
    }
}