using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class OptionalBooleanSearchActivator : SearchActivator<OptionalBooleanSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new OptionalBooleanSearch(itemProperty.Name);
    }
}