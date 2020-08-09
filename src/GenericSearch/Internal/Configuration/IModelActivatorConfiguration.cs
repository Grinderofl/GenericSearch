using System;

namespace GenericSearch.Internal.Configuration
{
    public interface IModelActivatorConfiguration
    {
        Type FactoryType { get; }
        Func<Type, object> Method { get; }
        Func<IServiceProvider, Type, object> Factory { get; }
    }
}