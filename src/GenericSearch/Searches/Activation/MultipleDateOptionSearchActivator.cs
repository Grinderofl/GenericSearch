using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleDateOptionSearchActivator : SearchActivator<MultipleDateOptionSearch>
    {
        public override ISearch Activate(string entityPath) => new MultipleDateOptionSearch(entityPath);
    }
}