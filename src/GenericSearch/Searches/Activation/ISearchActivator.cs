using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public interface ISearchActivator
    {
        ISearch Activate(string entityPath);
    }

    public interface ISearchActivator<TSearch> : ISearchActivator where TSearch : ISearch
    {
    }

}