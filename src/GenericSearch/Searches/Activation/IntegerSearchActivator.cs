using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class IntegerSearchActivator : SearchActivator<IntegerSearch>
    {
        public override ISearch Activate(string entityPath) => new IntegerSearch(entityPath);
    }
}