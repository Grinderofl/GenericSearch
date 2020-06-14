using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class TrueBooleanSearchActivator : SearchActivator<TrueBooleanSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new TrueBooleanSearch(itemProperty.Name);
    }
}