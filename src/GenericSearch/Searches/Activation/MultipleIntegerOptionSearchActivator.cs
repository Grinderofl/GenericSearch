using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleIntegerOptionSearchActivator : SearchActivator<MultipleIntegerOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new MultipleIntegerOptionSearch(itemProperty.Name);
    }
}