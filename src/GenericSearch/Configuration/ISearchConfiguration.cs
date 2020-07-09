using System;
using System.Reflection;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;

namespace GenericSearch.Configuration
{
    public interface ISearchConfiguration
    {
        bool Ignored { get; }
        PropertyInfo RequestProperty { get; }
        string ItemPropertyPath { get; }
        PropertyInfo ResultProperty { get; }
        Func<ISearch> Constructor { get; }
        Func<IServiceProvider, ISearchActivator> Activator { get; }
    }
}