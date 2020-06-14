using System;
using System.Linq;

namespace GenericSearch.Definition.Expressions
{
    public class RequestFactoryExpression : IRequestFactoryDefinition
    {
        public RequestFactoryExpression(Func<object> factoryMethod)
        {
            FactoryMethod = factoryMethod;
        }

        public RequestFactoryExpression(Func<IServiceProvider, object> factoryServiceProvider)
        {
            FactoryServiceProvider = factoryServiceProvider;
        }

        public RequestFactoryExpression(Type factoryType)
        {
            if (!factoryType.GetInterfaces().Contains(typeof(IRequestFactory)))
            {
                throw new ArgumentException($"{factoryType.FullName} does not implement {nameof(IRequestFactory)}.");
            }

            FactoryType = factoryType;
        }

        public Func<object> FactoryMethod { get; }
        public Func<IServiceProvider, object> FactoryServiceProvider { get; }
        public Type FactoryType { get; }
    }
}