using System;
using System.Linq.Expressions;
using GenericSearch.Searches;

namespace GenericSearch
{
    public interface ISearchTarget<TRequest, TResult>
    {
        ISearchTarget<TRequest, TResult> Text();

        ISearchTarget<TRequest, TResult> Ignore();

        ISearchTarget<TRequest, TResult> MapTo<T>(Expression<Func<TResult, T>> resultProperty) where T : ISearch;

        ISearchTarget<TRequest, TResult> MapTo(string resultPropertyName);

        ISearchTarget<TRequest, TResult> ConstructUsing(Func<ISearch> factoryMethod);
    }

    public interface IPropertyExpression<TRequest, TResult>
    {
        IPropertyExpression<TRequest, TResult> MapTo(Expression<Func<TResult, object>> resultProperty);
        IPropertyExpression<TRequest, TResult> MapTo(string resultPropertyName);
        IPropertyExpression<TRequest, TResult> DefaultValue(object defaultValue);
    }

    public interface ISortPropertyExpression<TRequest, TResult>
    {
        
    }

    public interface IConfigurationExpression<TItem, TRequest, TResult>
    {
        IConfigurationExpression<TItem, TRequest, TResult> Property<T>(Expression<Func<TRequest, T>> property, Action<ISearchTarget<TRequest, TResult>> action) where T : ISearch;
        
        IConfigurationExpression<TItem, TRequest, TResult> Property(Expression<Func<TRequest, object>> property, Action<IPropertyExpression<TRequest, TResult>> action);

    }

    public class ConfigurationExpression<TItem, TRequest, TResult> : IConfigurationExpression<TItem, TRequest, TResult>
    {
        public IConfigurationExpression<TItem, TRequest, TResult> Property<T>(Expression<Func<TRequest, T>> property, Action<ISearchTarget<TRequest, TResult>> action) where T : ISearch
        {
            throw new NotImplementedException();
        }

        public IConfigurationExpression<TItem, TRequest, TResult> Property(Expression<Func<TRequest, object>> property, Action<IPropertyExpression<TRequest, TResult>> action)
        {
            throw new NotImplementedException();
        }
    }


    public class Foo
    {
        public string Alice { get; set; }
        public int Bob { get; set; }
    }

    public class Bar
    {
        public TextSearch Alice { get; set; }
        public int Page { get; set; }
        public int Rows { get; set; }
    }

    public class Baz
    {
        public TextSearch Alice { get; set; }
        public int Page { get; set; }
        public int Rows { get; set; }
    }

    public class Starter
    {
        public Starter()
        {
            //Create<Foo, Bar, Baz>()
            //    .Property(x => x.Rows)
                
        }

        protected IConfigurationExpression<TItem, TRequest, TResult> Create<TItem, TRequest, TResult>()
        {
            return new ConfigurationExpression<TItem, TRequest, TResult>();
        }
    }

    /// <summary>
    /// Provides additional configuration options for querying <typeparamref name="TItem"/> using filters from
    /// <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TItem">Entity/Projection queryable type</typeparam>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface IFilterExpression<TItem, TRequest, TResult>
    {
        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as text search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Text(Expression<Func<TRequest, object>> propertyExpression, 
                                                         Action<ISearchExpression<TRequest, TResult>> criterionAction = null);

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single text option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> SingleTextOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                     Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi text option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> MultiTextOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                    Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as date search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Date(Expression<Func<TRequest, object>> propertyExpression,
                                                         Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single date option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> SingleDateOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                     Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi date option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> MultiDateOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                    Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as integer search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Integer(Expression<Func<TRequest, object>> propertyExpression,
                                                            Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single integer option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> SingleIntegerOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                        Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi integer option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> MultiIntegerOption(Expression<Func<TRequest, object>> propertyExpression,
                                                                       Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as decimal search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Decimal(Expression<Func<TRequest, object>> propertyExpression,
                                                            Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as boolean search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Boolean(Expression<Func<TRequest, object>> propertyExpression,
                                                            Action<ISearchExpression<TRequest, TResult>>
                                                                criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as optional boolean search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> OptionalBoolean(Expression<Func<TRequest, object>> propertyExpression,
                                                                    Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as custom search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="predicate">Predicate to use when creating the search property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Custom(Expression<Func<TRequest, object>> propertyExpression,
                                                           Func<ISearch> predicate, Action<ISearchExpression<TRequest, TResult>> criterionAction = null);


        /// <summary>
        /// Configures sort and direction properties
        /// </summary>
        /// <param name="sortAction">Action to perform for sort specifications</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Sort(Action<ISortExpression<TItem, TRequest, TResult>> sortAction);

        /// <summary>
        /// Configures page and rows properties
        /// </summary>
        /// <param name="pageAction">Action to perform for paging specifications</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> Page(Action<IPageExpression<TRequest, TResult>> pageAction);

        /// <summary>
        /// Configures POST to GET redirection
        /// </summary>
        /// <param name="postToGetRedirectAction">Action to perform for POST to GET redirect specification</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> RedirectPostToGet(Action<IRedirectPostToGetExpression<TRequest, TResult>> postToGetRedirectAction);

        /// <summary>
        /// Configures copying request filter values to result filter values
        /// </summary>
        /// <param name="copyRequestFilterValuesAction">Action to perform for copying request filter values specification</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> CopyRequestFilterValues(Action<ICopyRequestFilterValuesExpression<TRequest, TResult>> copyRequestFilterValuesAction);

        /// <summary>
        /// Configures the name of the List Action with the method parameter <typeparamref name="TRequest"/> which GenericSearch should
        /// perform POST to GET redirects and copying request filter values against.
        /// </summary>
        /// <param name="listActionName">Name of the list action</param>
        /// <returns>Filter expression</returns>
        IFilterExpression<TItem, TRequest, TResult> UseListActionName(string listActionName);

    }
}