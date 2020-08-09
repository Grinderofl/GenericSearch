using System;
using System.Reflection;

namespace GenericSearch.Internal.Definition
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