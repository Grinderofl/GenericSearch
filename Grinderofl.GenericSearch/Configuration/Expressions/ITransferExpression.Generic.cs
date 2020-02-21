#pragma warning disable 1591
using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    public interface ITransferExpression<TResult>
    {
        /// <summary>
        /// Specifies the transfer property on TResult
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ITransferExpression<TResult> WithResultProperty(Expression<Func<TResult, object>> expression);
    }
}