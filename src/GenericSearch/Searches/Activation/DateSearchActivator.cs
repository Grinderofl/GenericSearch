using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class DateSearchActivator : SearchActivator<DateSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new DateSearch(itemProperty.Name);
    }
}