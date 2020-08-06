using System;
using System.Linq;

namespace GenericSearch.Definition.Expressions
{
    public class ModelActivatorExpression : IModelActivatorDefinition
    {
        public ModelActivatorExpression(Func<Type, object> method)
        {
            Method = method;
        }

        public ModelActivatorExpression(Func<IServiceProvider, Type, object> factory)
        {
            Factory = factory;
        }

        public ModelActivatorExpression(Type factoryType)
        {
            if (!factoryType.GetInterfaces().Contains(typeof(IModelFactory)))
            {
                throw new ArgumentException($"{factoryType.FullName} does not implement {nameof(IModelFactory)}.");
            }

            FactoryType = factoryType;
        }

        public Func<Type, object> Method { get; }
        public Func<IServiceProvider, Type, object> Factory { get; }
        public Type FactoryType { get; }
    }
}