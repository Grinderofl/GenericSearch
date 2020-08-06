using System;
using System.Reflection;

namespace GenericSearch.Internal.Configuration
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