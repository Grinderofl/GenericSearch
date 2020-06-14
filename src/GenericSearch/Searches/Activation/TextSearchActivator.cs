using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class TextSearchActivator : SearchActivator<TextSearch>
    {
        public override ISearch Create(PropertyInfo itemProperty) => new TextSearch(itemProperty.Name);
    }
}