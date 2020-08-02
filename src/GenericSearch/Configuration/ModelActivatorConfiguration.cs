using System;

namespace GenericSearch.Configuration
{
    public class ModelActivatorConfiguration : IModelActivatorConfiguration
    {
        public ModelActivatorConfiguration(Type factoryType) => FactoryType = factoryType;
        public ModelActivatorConfiguration(Func<Type, object> factoryMethod) => Method = factoryMethod;
        public ModelActivatorConfiguration(Func<IServiceProvider, Type, object> factoryServiceProvider) => Factory = factoryServiceProvider;

        public ModelActivatorConfiguration(Type factoryType, Func<Type, object> method, Func<IServiceProvider, Type, object> factory)
        {
            FactoryType = factoryType;
            Method = method;
            Factory = factory;
        }

        public Type FactoryType { get; }
        public Func<Type, object> Method { get; }
        public Func<IServiceProvider, Type, object> Factory { get; }
    }
}