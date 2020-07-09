using System;

namespace GenericSearch.Configuration
{
    public interface IRequestFactoryConfiguration
    {
        Type FactoryType { get; }
        Func<Type, object> FactoryMethod { get; }
        Func<IServiceProvider, Type, object> FactoryServiceProvider { get; }
    }
}