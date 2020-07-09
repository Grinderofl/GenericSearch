using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleTextOptionSearchActivator : SearchActivator<SingleTextOptionSearch>
    {
        public override ISearch Activate(string entityPath) => new SingleTextOptionSearch(entityPath);
    }
}