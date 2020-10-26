using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleIntegerOptionSearchActivator : SearchActivator<SingleIntegerOptionSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new SingleIntegerOptionSearch(entityPaths);
    }
}