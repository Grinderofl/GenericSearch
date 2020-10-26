using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class DecimalSearchActivator : SearchActivator<DecimalSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new DecimalSearch(entityPaths);
    }
}