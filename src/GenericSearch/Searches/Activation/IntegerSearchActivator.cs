using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class IntegerSearchActivator : SearchActivator<IntegerSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new IntegerSearch(itemProperty.Name);
    }
}