using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleTextOptionSearchActivator : SearchActivator<MultipleTextOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new MultipleTextOptionSearch(itemProperty.Name);
    }
}