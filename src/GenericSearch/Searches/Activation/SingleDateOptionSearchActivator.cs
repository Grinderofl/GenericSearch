using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleDateOptionSearchActivator : SearchActivator<SingleDateOptionSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new SingleDateOptionSearch(itemProperty.Name);
    }
}