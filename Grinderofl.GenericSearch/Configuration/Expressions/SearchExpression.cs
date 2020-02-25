using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Helpers;
using Grinderofl.GenericSearch.Searches;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class SearchExpression<TRequest, TResult> : ISearchExpression, ISearchExpression<TRequest, TResult>
    {
        private readonly PropertyInfo searchProperty;

        public SearchExpression(PropertyInfo searchProperty, ISearch search)
        {
            this.searchProperty = searchProperty;
            Search = search;
        }

        public ISearch Search { get; }

        //private ISearch search;
        //public ISearch Search
        //{
        //    get { return search; }
        //    private set { search = value; }
        //}

        private PropertyInfo requestProperty;
        public PropertyInfo RequestProperty
        {
            get
            {
                if (requestProperty == null)
                {
                    requestProperty = PropertyInfoFactory.Create<TRequest>(searchProperty);
                }

                return requestProperty;
            }
        }

        private PropertyInfo resultProperty;
        public PropertyInfo ResultProperty
        {
            get
            {
                if (resultProperty == null)
                {
                    resultProperty = PropertyInfoFactory.Create<TResult>(RequestProperty);
                }

                return resultProperty;
            }
        }

        public ISearchExpression<TRequest, TResult> WithRequestProperty(Expression<Func<TRequest, object>> expression)
        {
            requestProperty = expression.GetPropertyInfo();
            return this;
        }

        public ISearchExpression<TRequest, TResult> WithResultProperty(Expression<Func<TResult, object>> expression)
        {
            resultProperty = expression.GetPropertyInfo();
            return this;
        }
    }
}