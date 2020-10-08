using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class IntegerSearchActivator : SearchActivator<IntegerSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new IntegerSearch(entityPaths);
    }
}