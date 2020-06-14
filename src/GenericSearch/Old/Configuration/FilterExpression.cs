using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;

namespace GenericSearch.Configuration
{
    /// <summary>
    /// Provides additional configuration options for querying <typeparamref name="TItem"/> using filters from
    /// <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TItem">Entity/Projection queryable type</typeparam>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class FilterExpression<TItem, TRequest, TResult> : IFilterConfiguration, 
                                                              IFilterExpression<TItem, TRequest, TResult>
    {
        /// <summary>
        /// Queryable type
        /// </summary>
        public Type ItemType => typeof(TItem);

        /// <summary>
        /// Request/Parameter type
        /// </summary>
        public Type RequestType => typeof(TRequest);

        /// <summary>
        /// Result/ViewModel type
        /// </summary>
        public Type ResultType => typeof(TResult);

        /// <summary>
        /// Configurations of the criteria on the filter
        /// </summary>
        public IList<ISearchConfiguration> SearchConfigurations { get; } = new List<ISearchConfiguration>();

        /// <summary>
        /// Configuration of sorting
        /// </summary>
        public ISortConfiguration SortConfiguration { get; private set; }

        /// <summary>
        /// Configuration of paging
        /// </summary>
        public IPageConfiguration PageConfiguration { get; private set; }

        /// <summary>
        /// Configuration of post to get redirection
        /// </summary>
        public IRedirectPostToGetConfiguration RedirectPostToGetConfiguration { get; private set; }

        /// <summary>
        /// Configuration of copying request filter values to result filters
        /// </summary>
        public ICopyRequestFilterValuesConfiguration CopyRequestFilterValuesConfiguration { get; private set; }

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against.
        /// </summary>
        public string ListActionName { get; private set; }


        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as text search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Text(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new TextSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single text option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> SingleTextOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new SingleTextOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi text option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> MultiTextOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new MultipleTextOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as date search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Date(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new DateSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single date option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> SingleDateOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new SingleDateOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi date option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> MultiDateOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new MultipleDateOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as integer search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Integer(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new IntegerSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as single integer option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> SingleIntegerOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new SingleIntegerOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as multi integer option search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> MultiIntegerOption(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new MultipleIntegerOptionSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as decimal search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Decimal(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new DecimalSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as boolean search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Boolean(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new BooleanSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as optional boolean search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> OptionalBoolean(Expression<Func<TRequest, object>> propertyExpression, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(() => new OptionalBooleanSearch(propertyType.Name), propertyType, criterionAction);
        }

        /// <summary>
        /// Configures property on <typeparamref name="TRequest"/> as custom search.
        /// </summary>
        /// <param name="propertyExpression">Search criterion property</param>
        /// <param name="predicate">Predicate to use when creating the search property</param>
        /// <param name="criterionAction">Action to perform on criterion</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Custom(Expression<Func<TRequest, object>> propertyExpression, Func<ISearch> predicate, Action<ISearchExpression<TRequest, TResult>> criterionAction = null)
        {
            var propertyType = propertyExpression.GetPropertyInfo();
            return CreateCriterion(predicate, propertyType, criterionAction);
        }

        /// <summary>
        /// Configures sort and direction properties
        /// </summary>
        /// <param name="sortAction">Action to perform for sort specifications</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Sort(Action<ISortExpression<TItem, TRequest, TResult>> sortAction)
        {
            var sortConfiguration = new SortExpression<TItem, TRequest, TResult>();
            sortAction(sortConfiguration);
            SortConfiguration = sortConfiguration;
            return this;
        }

        /// <summary>
        /// Configures page and rows properties
        /// </summary>
        /// <param name="pageAction">Action to perform for paging specifications</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> Page(Action<IPageExpression<TRequest, TResult>> pageAction)
        {
            var pageConfiguration = new PageExpression<TRequest, TResult>();
            pageAction(pageConfiguration);
            PageConfiguration = pageConfiguration;
            return this;
        }


        /// <summary>
        /// Configures POST to GET redirection
        /// </summary>
        /// <param name="postToGetRedirectAction">Action to perform for POST to GET redirect specification</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> RedirectPostToGet(Action<IRedirectPostToGetExpression<TRequest, TResult>> postToGetRedirectAction)
        {
            var redirectConfiguration = new RedirectPostToGetExpression<TRequest, TResult>();
            postToGetRedirectAction(redirectConfiguration);
            RedirectPostToGetConfiguration = redirectConfiguration;
            return this;
        }

        /// <summary>
        /// Configures copying request filter values to result filter values
        /// </summary>
        /// <param name="copyRequestFilterValuesAction">Action to perform for copying request filter values specification</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> CopyRequestFilterValues(Action<ICopyRequestFilterValuesExpression<TRequest, TResult>> copyRequestFilterValuesAction)
        {
            var copyRequestFilterValuesConfiguration = new CopyRequestFilterValuesExpression<TRequest, TResult>();
            copyRequestFilterValuesAction(copyRequestFilterValuesConfiguration);
            CopyRequestFilterValuesConfiguration = copyRequestFilterValuesConfiguration;
            return this;
        }

        /// <summary>
        /// Configures the name of the List Action with the method parameter <typeparamref name="TRequest"/> which GenericSearch should
        /// perform POST to GET redirects and copying request filter values against.
        /// </summary>
        /// <param name="listActionName">Name of the list action</param>
        /// <returns>Filter expression</returns>
        public IFilterExpression<TItem, TRequest, TResult> UseListActionName(string listActionName)
        {
            ListActionName = listActionName;
            return this;
        }

        private IFilterExpression<TItem, TRequest, TResult> CreateCriterion(Func<ISearch> predicate, PropertyInfo propertyType, Action<ISearchExpression<TRequest, TResult>> action = null)
        {
            var criterionExpression = new SearchExpression<TRequest, TResult>(predicate, propertyType);
            action?.Invoke(criterionExpression);
            SearchConfigurations.Add(criterionExpression);
            return this;
        }


    }
}