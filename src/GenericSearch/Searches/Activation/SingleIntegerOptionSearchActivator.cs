using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleIntegerOptionSearchActivator : SearchActivator<SingleIntegerOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new SingleIntegerOptionSearch(itemProperty.Name);
    }
}