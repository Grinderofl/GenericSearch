using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Internal.Definition.Expressions
{
    public class SearchExpression<TRequest, TEntity, TResult> : ISearchDefinition, ISearchExpression<TRequest, TEntity, TResult>
    {
        public PropertyInfo RequestProperty { get; }

        public bool Ignored { get; private set; }

        public string[] ItemPropertyPaths { get; private set; }

        public PropertyInfo ResultProperty { get; private set; }

        public Func<ISearch> Constructor { get; private set; }
        public Func<IServiceProvider, ISearchActivator> Activator { get; private set; }
        public Type ActivatorType { get; private set; }

        public SearchExpression(Expression<Func<TRequest, ISearch>> property)
        {
            RequestProperty = property.GetPropertyInfo();
        }
        
        public ISearchExpression<TRequest, TEntity, TResult> Ignore()
        {
            Ignored = true;
            return this;
        }

        public ISearchExpression<TRequest, TEntity, TResult> MapTo(Expression<Func<TResult, ISearch>> property)
        {
            ResultProperty = property.GetPropertyInfo();
            return this;
        }

        public ISearchExpression<TRequest, TEntity, TResult> On(params Expression<Func<TEntity, object>>[] properties)
        {
            var propertyPaths = properties.Select(x => string.Join(".", x.GetPropertyPath())).ToArray();
            return On(propertyPaths);
        }

        public ISearchExpression<TRequest, TEntity, TResult> On(params string[] propertyPaths)
        {
            ItemPropertyPaths = propertyPaths;
            return this;
        }

        public ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Func<ISearch> factoryMethod)
        {
            Constructor = factoryMethod;
            return this;
        }

        public ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Func<IServiceProvider, ISearchActivator> activator)
        {
            Activator = activator;
            return this;
        }

        public ISearchExpression<TRequest, TEntity, TResult> ActivateUsing<TActivator>() where TActivator : ISearchActivator
        {
            return ActivateUsing(typeof(TActivator));
        }

        public ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Type activatorType)
        {
            ActivatorType = activatorType;
            return this;
        }
    }
}