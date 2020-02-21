using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class TransferExpression<TResult> : ITransferExpression, ITransferExpression<TResult>
    {
        public TransferExpression(PropertyInfo queryProperty)
        {
            RequestProperty = queryProperty;
        }

        public PropertyInfo RequestProperty { get; }

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

        public ITransferExpression<TResult> WithResultProperty(Expression<Func<TResult, object>> expression)
        {
            resultProperty = expression.GetPropertyInfo();
            return this;
        }
    }
}