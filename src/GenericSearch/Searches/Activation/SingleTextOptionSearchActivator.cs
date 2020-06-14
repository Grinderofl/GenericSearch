using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleTextOptionSearchActivator : SearchActivator<SingleTextOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new SingleTextOptionSearch(itemProperty.Name);
    }
}