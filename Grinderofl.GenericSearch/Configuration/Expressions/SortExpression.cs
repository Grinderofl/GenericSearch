using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class SortExpression<TEntity, TRequest, TResult> : ISortExpression, ISortExpression<TEntity, TRequest, TResult>
    {
        private PropertyInfo requestSortByProperty;
        public PropertyInfo RequestSortByProperty
        {
            get
            {
                if (requestSortByProperty == null)
                {
                    requestSortByProperty = PropertyInfoFactory.Create<TRequest>(GenericSearchConstants.SortByPropertyName);
                }

                return requestSortByProperty;
            }
        }

        private PropertyInfo requestSortDirectionProperty;
        public PropertyInfo RequestSortDirectionProperty
        {
            get
            {
                if (requestSortDirectionProperty == null)
                {
                    requestSortDirectionProperty = PropertyInfoFactory.Create<TRequest>(GenericSearchConstants.SortDirectionPropertyName);
                }

                return requestSortDirectionProperty;
            }
        }

        private PropertyInfo resultSortByProperty;
        public PropertyInfo ResultSortByProperty
        {
            get
            {
                if (resultSortByProperty == null)
                {
                    resultSortByProperty = PropertyInfoFactory.Create<TResult>(RequestSortByProperty);
                }

                return resultSortByProperty;
            }
        }

        private PropertyInfo resultSortDirectionProperty;
        public PropertyInfo ResultSortDirectionProperty
        {
            get
            {
                if (resultSortDirectionProperty == null)
                {
                    resultSortDirectionProperty = PropertyInfoFactory.Create<TResult>(RequestSortDirectionProperty);
                }

                return resultSortDirectionProperty;
            }
        }

        public Direction DefaultSortDirection { get; private set; }
        public PropertyInfo DefaultSortBy { get; private set; }


        public ISortExpression<TEntity, TRequest, TResult> WithRequestSortByProperty(Expression<Func<TRequest, object>> expression)
        {
            requestSortByProperty = expression.GetPropertyInfo();
            return this;
        }

        public ISortExpression<TEntity, TRequest, TResult> WithRequestSortDirectionProperty(Expression<Func<TRequest, object>> expression)
        {
            requestSortDirectionProperty = expression.GetPropertyInfo();
            return this;
        }

        public ISortExpression<TEntity, TRequest, TResult> WithResultSortByProperty(Expression<Func<TResult, object>> expression)
        {
            resultSortByProperty = expression.GetPropertyInfo();
            return this;
        }

        public ISortExpression<TEntity, TRequest, TResult> WithResultSortDirectionProperty(Expression<Func<TResult, object>> expression)
        {
            resultSortDirectionProperty = expression.GetPropertyInfo();
            return this;
        }

        public ISortExpression<TEntity, TRequest, TResult> WithDefaultSortBy(Expression<Func<TEntity, object>> expression)
        {
            DefaultSortBy = expression.GetPropertyInfo();
            return this;
        }

        public ISortExpression<TEntity, TRequest, TResult> WithDefaultSortDirection(Direction direction)
        {
            DefaultSortDirection = direction;
            return this;
        }
    }
}