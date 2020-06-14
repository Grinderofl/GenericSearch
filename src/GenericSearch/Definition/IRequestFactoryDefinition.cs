using System;

namespace GenericSearch.Definition
{
    public interface IRequestFactoryDefinition
    {
        Func<object> FactoryMethod { get; }
        Func<IServiceProvider, object> FactoryServiceProvider { get; }
        Type FactoryType { get; }
    }
}