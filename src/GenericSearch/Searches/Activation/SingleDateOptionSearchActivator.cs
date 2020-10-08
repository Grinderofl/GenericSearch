using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleDateOptionSearchActivator : SearchActivator<SingleDateOptionSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new SingleDateOptionSearch(entityPaths);
    }
}