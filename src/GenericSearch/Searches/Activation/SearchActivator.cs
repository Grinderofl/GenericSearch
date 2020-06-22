using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public abstract class SearchActivator<TSearch> : ISearchActivator<TSearch> where TSearch : ISearch
    {
        public abstract ISearch Activate(string entityPath);
    }
}