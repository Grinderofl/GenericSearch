using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleIntegerOptionSearchActivator : SearchActivator<MultipleIntegerOptionSearch>
    {
        public override ISearch Activate(string entityPath) => new MultipleIntegerOptionSearch(entityPath);
    }
}