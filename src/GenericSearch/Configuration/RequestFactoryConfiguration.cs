using System;

namespace GenericSearch.Configuration
{
    public class RequestFactoryConfiguration
    {
        public RequestFactoryConfiguration()
        {
        }
        public RequestFactoryConfiguration(Type factoryType) => FactoryType = factoryType;
        public RequestFactoryConfiguration(Func<Type, object> factoryMethod) => FactoryMethod = factoryMethod;
        public RequestFactoryConfiguration(Func<IServiceProvider, Type, object> factoryServiceProvider) => FactoryServiceProvider = factoryServiceProvider;

        public Type FactoryType { get; set; }
        public Func<Type, object> FactoryMethod { get; set; }
        public Func<IServiceProvider, Type, object> FactoryServiceProvider { get; set; }
    }
}