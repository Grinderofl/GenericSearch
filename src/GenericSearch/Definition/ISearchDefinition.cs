using System;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;

namespace GenericSearch.Definition
{
    public interface ISearchDefinition
    {
        bool Ignored { get; }
        PropertyInfo RequestProperty { get; }
        string ItemPropertyPath { get; }
        
        //PropertyInfo EntityPath { get; }
        PropertyInfo ResultProperty { get; }
        Func<ISearch> Constructor { get; }
        Func<IServiceProvider, ISearchActivator> Activator { get; }
        Type ActivatorType { get; }
    }
}