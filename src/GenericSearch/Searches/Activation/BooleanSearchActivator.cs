using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class BooleanSearchActivator : SearchActivator<BooleanSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new BooleanSearch(entityPaths);
    }
}