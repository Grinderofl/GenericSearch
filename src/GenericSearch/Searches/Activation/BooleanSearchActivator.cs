using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class BooleanSearchActivator : SearchActivator<BooleanSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new BooleanSearch(itemProperty.Name);
    }
}