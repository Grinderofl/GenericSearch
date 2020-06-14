using System;
using System.Linq.Expressions;
using GenericSearch.Definition;
using GenericSearch.Searches;

namespace GenericSearch
{
    public interface IListExpression<TRequest, TItem, TResult>
    {
        IListExpression<TRequest, TItem, TResult> Search(Expression<Func<TRequest, ISearch>> property,
                                                         Action<ISearchExpression<TRequest, TItem, TResult>> action =
                                                             null);

        IListExpression<TRequest, TItem, TResult> Property<T>(Expression<Func<TRequest, T>> property,
                                                              Action<IPropertyExpression<TRequest, T, TResult>> action =
                                                                  null);

        IListExpression<TRequest, TItem, TResult> SortColumn(Expression<Func<TRequest, string>> property,
                                                             Action<ISortColumnExpression<TRequest, TItem, TResult>>
                                                                 action = null);

        IListExpression<TRequest, TItem, TResult> SortColumn(string name = null,
                                                             Action<ISortColumnExpression<TRequest, TItem, TResult>>
                                                                 action = null);


        IListExpression<TRequest, TItem, TResult> SortDirection(Expression<Func<TRequest, Direction>> property,
                                                                Action<ISortDirectionExpression<TRequest, TResult>>
                                                                    action = null);

        IListExpression<TRequest, TItem, TResult> SortDirection(string name = null,
                                                                Action<ISortDirectionExpression<TRequest, TResult>>
                                                                    action = null);

        /// <summary>
        /// Explicitly configures the page number property on <typeparamref name="TRequest"/>.
        /// </summary>
        /// <param name="property">Page number property on <typeparamref name="TRequest"/></param>
        /// <param name="action">Additional configuration for page number property on <typeparamref name="TResult"/> and the default page value.</param>
        /// <returns></returns>
        IListExpression<TRequest, TItem, TResult> Page(Expression<Func<TRequest, int>> property,
                                                       Action<IPageExpression<TRequest, TResult>> action = null);

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

        IListExpression<TRequest, TItem, TResult> Rows(Expression<Func<TRequest, int>> property,
                                                       Action<IRowsExpression<TRequest, TResult>> action = null);

        IListExpression<TRequest, TItem, TResult> Rows(string name, Action<IRowsExpression> action = null);
        IListExpression<TRequest, TItem, TResult> Rows(int? defaultValue = null);

        IListExpression<TRequest, TItem, TResult> PostRedirectGet(Action<IPostRedirectGetExpression> action);
        IListExpression<TRequest, TItem, TResult> TransferValues(Action<ITransferValuesExpression> action);
        IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<object> factoryMethod);
        IListExpression<TRequest, TItem, TResult> ConstructUsing<T>() where T : IRequestFactory;

        IListExpression<TRequest, TItem, TResult> ConstructUsing(Func<IServiceProvider, object> activator);
    }
}

    