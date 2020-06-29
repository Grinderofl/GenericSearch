using System;
using System.Linq.Expressions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;

namespace GenericSearch
{
    public interface ISearchExpression<TRequest, TEntity, TResult>
    {
        /// <summary>
        /// Configures the request property to be ignored when searching, sorting, or paging.
        /// </summary>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> Ignore();

        /// <summary>
        /// Configures the request property to be mapped to a specific result property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>

        ISearchExpression<TRequest, TEntity, TResult> MapTo(Expression<Func<TResult, ISearch>> property);

        /// <summary>
        /// Configures the entity property to use when performing search.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> On(Expression<Func<TEntity, object>> property);

        /// <summary>
        /// Configures the entity property to use when performing search.
        /// </summary>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> On(string propertyPath);

        /// <summary>
        /// Configures the request property to be activated using a factory method.
        /// </summary>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Func<ISearch> factoryMethod);

        /// <summary>
        /// Configures the request property to be activated using a service provider factory method.
        /// </summary>
        /// <param name="activator"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Func<IServiceProvider, ISearchActivator> activator);

        /// <summary>
        /// Configures the request property to be activated using an implementation of <see cref="ISearchActivator{TSearch}"/>.
        /// </summary>
        /// <typeparam name="TActivator"></typeparam>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> ActivateUsing<TActivator>() where TActivator : ISearchActivator;

        /// <summary>
        /// Configures the request property to be activated using an implementation of <see cref="ISearchActivator{TSearch}"/>.
        /// </summary>
        /// <param name="activatorType"></param>
        /// <returns></returns>
        ISearchExpression<TRequest, TEntity, TResult> ActivateUsing(Type activatorType);
    }
}