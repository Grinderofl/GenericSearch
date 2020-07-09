using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.Definition.Expressions
{
    public class SearchExpression<TRequest, TEntity, TResult> : ISearchDefinition, ISearchExpression<TRequest, TEntity, TResult>
    {
        public PropertyInfo RequestProperty { get; }

        public bool Ignored { get; private set; }

        public string ItemPropertyPath { get; private set; }

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

        public ISearchExpression<TRequest, TEntity, TResult> On(Expression<Func<TEntity, object>> property)
        {
            var propertyPath = property.GetPropertyPath();
            //var expression = property.ToString();
            //var propertyPath = expression.Split(".").Skip(1);
            return On(string.Join(".", propertyPath));
        }

        public ISearchExpression<TRequest, TEntity, TResult> On(string propertyPath)
        {
            ItemPropertyPath = propertyPath;
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