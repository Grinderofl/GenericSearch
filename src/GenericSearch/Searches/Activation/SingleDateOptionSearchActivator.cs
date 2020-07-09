using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class SingleDateOptionSearchActivator : SearchActivator<SingleDateOptionSearch>
    {
        public override ISearch Activate(string entityPath) => new SingleDateOptionSearch(entityPath);
    }
}