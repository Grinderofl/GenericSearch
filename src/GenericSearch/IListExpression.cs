using System;
using System.Linq.Expressions;
using GenericSearch.Definition;
using GenericSearch.Searches;

namespace GenericSearch
{
    public interface IListExpression<TRequest, TItem, TResult>
    {
        /// <summary>
        /// Explicitly configures a search property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Search(Expression<Func<TRequest, ISearch>> property, Action<ISearchExpression<TRequest, TItem, TResult>> action = null);

        /// <summary>
        /// Explicitly configures a property which is not recognised by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>

        IListExpression<TRequest, TItem, TResult> Property<T>(Expression<Func<TRequest, T>> property, Action<IPropertyExpression<TRequest, T, TResult>> action = null);

        /// <summary>
        /// Explicitly configures the sort column property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> SortColumn(Expression<Func<TRequest, string>> property, Action<ISortColumnExpression<TRequest, TItem, TResult>> action = null);

        /// <summary>
        /// Explicitly configures the sort column property.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>

        IListExpression<TRequest, TItem, TResult> SortColumn(string name = null, Action<ISortColumnExpression<TRequest, TItem, TResult>> action = null);

        /// <summary>
        /// Explicitly configures the sort direction property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>

        IListExpression<TRequest, TItem, TResult> SortDirection(Expression<Func<TRequest, Direction>> property, Action<ISortDirectionExpression<TRequest, TResult>> action = null);

        /// <summary>
        /// Explicitly configures the sort direction property.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> SortDirection(string name = null, Action<ISortDirectionExpression<TRequest, TResult>> action = null);

        /// <summary>
        /// Explicitly configures the page number property on <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="property">Page number property on <typeparamref name="TRequest"/></param>
        /// <param name="action">Additional configuration for page number property on <typeparamref name="TResult"/> and the default page value.</param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Page(Expression<Func<TRequest, int>> property, Action<IPageExpression<TRequest, TResult>> action = null);

        /// <summary>
        /// Configures the page number query string parameter name.
        /// <example>
        /// ?foo.Term=filter&amp;page=2
        /// </example>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action">Additional configuration for the default page value.</param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Page(string name, Action<IPageExpression> action = null);

        /// <summary>
        /// Configures the page number property or query string parameter name by the default value specified in <see cref="GenericSearchOptions"/> and the default page value.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Page(int? defaultValue = null);

        /// <summary>
        /// Configures the rows per page property on <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Rows(Expression<Func<TRequest, int>> property, Action<IRowsExpression<TRequest, TResult>> action = null);

        /// <summary>
        /// Configures the rows per page query string parameter name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Rows(string name, Action<IRowsExpression> action = null);

        /// <summary>
        /// Configures the rows per page property or query string parameter name by the default value specified in <see cref="GenericSearchOptions"/> and the default rows per page value.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Rows(int? defaultValue = null);

        /// <summary>
        /// Configures the POST-redirect-GET behaviour for <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> PostRedirectGet(Action<IPostRedirectGetExpression> action);

        /// <summary>
        /// Configures the Transfer Values behaviour from <typeparamref name="TRequest"/> to <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> TransferValues(Action<ITransferValuesExpression> action);

        /// <summary>
        /// Configures the model binder to use a factory method when activating <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<object> factoryMethod);

        /// <summary>
        /// Configures the model binder to use a <see cref="IRequestFactory"/> when activating <typeparamref name="TRequest"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> ConstructUsing<T>() where T : IRequestFactory;

        /// <summary>
        /// Configure the model binder to ue a service provider factory method when activating <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="activator"></param>
        /// <returns></returns>

        IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<IServiceProvider, object> activator);
    }
}

    