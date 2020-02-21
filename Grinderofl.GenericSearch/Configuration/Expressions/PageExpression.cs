using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class PageExpression<TRequest, TResult> : IPageExpression, IPageExpression<TRequest, TResult>
    {
        private PropertyInfo requestPageProperty;

        public PropertyInfo RequestPageProperty
        {
            get
            {
                if (requestPageProperty == null)
                {
                    requestPageProperty = PropertyInfoFactory.Create<TRequest>(GenericSearchConstants.PagePropertyName);
                }

                return requestPageProperty;
            }
        }

        private PropertyInfo requestRowsProperty;

        public PropertyInfo RequestRowsProperty
        {
            get
            {
                if (requestRowsProperty == null)
                {
                    requestRowsProperty = PropertyInfoFactory.Create<TRequest>(GenericSearchConstants.RowsPropertyName);
                }

                return requestRowsProperty;
            }
        }

        private PropertyInfo resultPageProperty;

        public PropertyInfo ResultPageProperty
        {
            get
            {
                if (resultPageProperty == null)
                {
                    resultPageProperty = PropertyInfoFactory.Create<TResult>(RequestPageProperty);
                }

                return resultPageProperty;
            }
        }

        private PropertyInfo resultRowsProperty;

        public PropertyInfo ResultRowsProperty
        {
            get
            {
                if (resultRowsProperty == null)
                {
                    resultRowsProperty = PropertyInfoFactory.Create<TResult>(RequestRowsProperty);
                }

                return resultRowsProperty;
            }
        }

        public int DefaultRows { get; private set; }
        public int DefaultPage { get; private set; } = 1;

        public IPageExpression<TRequest, TResult> WithRequestPageProperty(Expression<Func<TRequest, object>> expression)
        {
            requestPageProperty = expression.GetPropertyInfo();
            return this;
        }

        public IPageExpression<TRequest, TResult> WithRequestRowsProperty(Expression<Func<TRequest, object>> expression)
        {
            requestRowsProperty = expression.GetPropertyInfo();
            return this;
        }

        public IPageExpression<TRequest, TResult> WithResultPageProperty(Expression<Func<TResult, object>> expression)
        {
            resultPageProperty = expression.GetPropertyInfo();
            return this;
        }

        public IPageExpression<TRequest, TResult> WithResultRowsProperty(Expression<Func<TResult, object>> expression)
        {
            resultRowsProperty = expression.GetPropertyInfo();
            return this;
        }

        public IPageExpression<TRequest, TResult> WithDefaultRows(int rows)
        {
            DefaultRows = rows;
            return this;
        }

        public IPageExpression<TRequest, TResult> WithDefaultPage(int page)
        {
            DefaultPage = page;
            return this;
        }
    }
}