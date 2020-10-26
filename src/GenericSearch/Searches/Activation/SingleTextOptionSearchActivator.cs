using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleTextOptionSearchActivator : SearchActivator<SingleTextOptionSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new SingleTextOptionSearch(entityPaths);
    }
}