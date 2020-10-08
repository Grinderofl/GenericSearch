using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class TrueBooleanSearchActivator : SearchActivator<TrueBooleanSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new TrueBooleanSearch(entityPaths);
    }
}