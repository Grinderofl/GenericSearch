using System;
using System.Linq.Expressions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;

namespace GenericSearch
{
    public interface ISearchExpression<TRequest, TItem, TResult>
    {
        ISearchExpression<TRequest, TItem, TResult> Ignore();

        ISearchExpression<TRequest, TItem, TResult> MapTo(Expression<Func<TResult, ISearch>> property);

        ISearchExpression<TRequest, TItem, TResult> On(Expression<Func<TItem, object>> property);
        ISearchExpression<TRequest, TItem, TResult> On(string propertyPath);
        ISearchExpression<TRequest, TItem, TResult> ConstructUsing(Func<ISearch> factoryMethod);
        ISearchExpression<TRequest, TItem, TResult> ActivateUsing(Func<IServiceProvider, ISearchActivator> activator);
        ISearchExpression<TRequest, TItem, TResult> ActivateUsing<TActivator>() where TActivator : ISearchActivator;
        ISearchExpression<TRequest, TItem, TResult> ActivateUsing(Type activatorType);
    }
}