using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class OptionalBooleanSearchActivator : SearchActivator<OptionalBooleanSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new OptionalBooleanSearch(entityPaths);
    }
}