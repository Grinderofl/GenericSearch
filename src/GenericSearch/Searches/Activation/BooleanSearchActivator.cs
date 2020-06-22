using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class BooleanSearchActivator : SearchActivator<BooleanSearch>
    {
        public override ISearch Activate(string entityPath) => new BooleanSearch(entityPath);
    }
}