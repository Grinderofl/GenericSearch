using System;

namespace GenericSearch.Internal.Definition
{
    public interface IModelActivatorDefinition
    {
        Func<Type, object> Method { get; }
        Func<IServiceProvider, Type, object> Factory { get; }
        Type FactoryType { get; }
    }
}