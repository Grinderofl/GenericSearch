#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration.Expressions;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Searches;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Base class for defining a generic search profile.
    /// </summary>
    /// <typeparam name="TEntity">Type of the list item (queryable) in the result model.</typeparam>
    /// <typeparam name="TRequest">Type of the request / query model.</typeparam>
    /// <typeparam name="TResult">Type of the result / view model.</typeparam>
    public abstract class SearchProfile<TEntity, TRequest, TResult> : SearchConfigurationBase
    {
        protected SearchProfile()
        {
            EntityType = typeof(TEntity);
            RequestType = typeof(TRequest);
            ResultType = typeof(TResult);
        }

        /// <summary>
        /// Adds a generic text search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression">Model Item property for TextSearch.</param>
        public ISearchExpression<TRequest, TResult> AddText(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new TextSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        /// <summary>
        /// Adds a generic text select list search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="multiple">Whether to allow selecting multiple values, defaults to true.</param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddTextOption(Expression<Func<TEntity, object>> expression, bool multiple = true)
        {
            var propertyInfo = expression.GetPropertyInfo();
            var search = multiple
                             ? SearchFactory.Create<MultipleTextOptionSearch>(propertyInfo)
                             : SearchFactory.Create<SingleTextOptionSearch>(propertyInfo);

            return AddSearchProperty(propertyInfo, search);
        }

        /// <summary>
        /// Adds a generic date search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddDate(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new DateSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        /// <summary>
        /// Adds a generic date select list search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="multiple">Whether to allow selecting multiple values, defaults to true.</param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddDateOption(Expression<Func<TEntity, object>> expression, bool multiple = true)
        {
            var propertyInfo = expression.GetPropertyInfo();
            var search = multiple
                             ? SearchFactory.Create<MultipleDateOptionSearch>(propertyInfo)
                             : SearchFactory.Create<SingleDateOptionSearch>(propertyInfo);

            return AddSearchProperty(propertyInfo, search);
        }

        /// <summary>
        /// Adds a generic integer search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddInteger(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new IntegerSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        /// <summary>
        /// Adds a generic integer select list search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="multiple">Whether to allow selecting multiple values, defaults to true.</param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddIntegerOption(Expression<Func<TEntity, object>> expression, bool multiple = true)
        {
            var propertyInfo = expression.GetPropertyInfo();
            var search = multiple
                             ? SearchFactory.Create<MultipleIntegerOptionSearch>(propertyInfo)
                             : SearchFactory.Create<SingleIntegerOptionSearch>(propertyInfo);
            return AddSearchProperty(propertyInfo, search);
        }

        /// <summary>
        /// Adds a generic decimal search over the provided <typeparamref name="TEntity"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddDecimal(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new DecimalSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        public ISearchExpression<TRequest, TResult> AddBoolean(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new BooleanSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        public ISearchExpression<TRequest, TResult> AddTrueBoolean(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new TrueBooleanSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        public ISearchExpression<TRequest, TResult> AddOptionalBoolean(Expression<Func<TEntity, object>> expression)
        {
            var entityPropertyInfo = expression.GetPropertyInfo();
            var search = new OptionalBooleanSearch(entityPropertyInfo.Name);
            return AddSearchProperty(entityPropertyInfo, search);
        }

        /// <summary>
        /// Adds a custom search over the provider <typeparamref name="TRequest"/> property.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddCustom(Expression<Func<TRequest, object>> expression)
        {
            var requestPropertyInfo = expression.GetPropertyInfo();
            var search = SearchFactory.Create(requestPropertyInfo);
            return AddCustom(search, expression);
        }

        /// <summary>
        /// Adds a custom search over the provider <typeparamref name="TRequest"/> property.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISearchExpression<TRequest, TResult> AddCustom(ISearch search, Expression<Func<TRequest, object>> expression)
        {
            return AddCustomSearchProperty(expression, search);
        }
        
        /// <summary>
        /// Adds generic sorting functionality.
        /// </summary>
        /// <returns></returns>
        public ISortExpression<TEntity, TRequest, TResult> AddSort()
        {
            var sortExpression = new SortExpression<TEntity, TRequest, TResult>();
            SortExpression = sortExpression;
            return sortExpression;
        }

        /// <summary>
        /// Adds generic paging functionality.
        /// </summary>
        /// <returns></returns>
        public IPageExpression<TRequest, TResult> AddPaging()
        {
            var pageExpression = new PageExpression<TRequest, TResult>();
            PageExpression = pageExpression;
            return pageExpression;
        }

        /// <summary>
        /// Marks the provided <typeparamref name="TRequest"/> property value to be transferred to <typeparamref name="TResult"/> property.
        /// </summary>
        /// <param name="expression">Property to transfer</param>
        public ITransferExpression<TResult> AddTransfer(Expression<Func<TRequest, object>> expression)
        {
            if (TransferExpressions == null)
            {
                TransferExpressions = new List<ITransferExpression>();
            }

            var requestPropertyInfo = expression.GetPropertyInfo();
            var transferExpression = new TransferExpression<TResult>(requestPropertyInfo);
            TransferExpressions.Add(transferExpression);
            return transferExpression;
        }

        /// <summary>
        /// Specifies whether POST actions with TRequest parameter should be automatically redirected with their properties
        /// transformed to route values.
        /// </summary>
        /// <param name="behaviour"></param>
        public void RedirectPostRequests(ProfileBehaviour behaviour)
        {
            RedirectBehaviour = behaviour;
        }

        /// <summary>
        /// Specifies whether property values from TRequest should be automatically transferred to the result view model
        /// properties after an action has finished executing.
        /// </summary>
        /// <param name="behaviour"></param>
        public void TransferRequestProperties(ProfileBehaviour behaviour)
        {
            TransferBehaviour = behaviour;
        }

        private ISearchExpression<TRequest, TResult> AddCustomSearchProperty(Expression<Func<TRequest, object>> expression, ISearch search)
        {
            if (CustomSearchExpressions == null)
            {
                CustomSearchExpressions = new List<ISearchExpression>();
            }

            var requestPropertyInfo = expression.GetPropertyInfo();
            var searchExpression = new SearchExpression<TRequest, TResult>(requestPropertyInfo, search);
            searchExpression.WithRequestProperty(expression);
            CustomSearchExpressions.Add(searchExpression);
            return searchExpression;
        }

        private ISearchExpression<TRequest, TResult> AddSearchProperty(PropertyInfo entityPropertyInfo, ISearch search)
        {
            if (SearchExpressions == null)
            {
                SearchExpressions = new List<ISearchExpression>();
            }

            var searchExpression = new SearchExpression<TRequest, TResult>(entityPropertyInfo, search);
            SearchExpressions.Add(searchExpression);
            return searchExpression;
        }
    }
}