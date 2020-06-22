using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleIntegerOptionSearchActivator : SearchActivator<SingleIntegerOptionSearch>
    {
        public override ISearch Activate(string entityPath) => new SingleIntegerOptionSearch(entityPath);
    }
}