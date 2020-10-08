using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleDateOptionSearchActivator : SearchActivator<MultipleDateOptionSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new MultipleDateOptionSearch(entityPaths);
    }
}