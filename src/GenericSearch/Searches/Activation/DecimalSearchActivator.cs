using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class DecimalSearchActivator : SearchActivator<DecimalSearch>
    {
        public override ISearch Activate(string entityPath) => new DecimalSearch(entityPath);
    }
}