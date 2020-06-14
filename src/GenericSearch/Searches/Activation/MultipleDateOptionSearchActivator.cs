using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleDateOptionSearchActivator : SearchActivator<MultipleDateOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new MultipleDateOptionSearch(itemProperty.Name);
    }
}