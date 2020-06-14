using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class DecimalSearchActivator : SearchActivator<DecimalSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new DecimalSearch(itemProperty.Name);
    }
}