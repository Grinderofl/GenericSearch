using System.Collections.Generic;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class TextSearchActivator : SearchActivator<TextSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new TextSearch(entityPaths);
    }
}
