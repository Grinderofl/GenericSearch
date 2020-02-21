using System;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Helpers
{
    /// <summary>
    /// Factory to create an Expression&lt;Func&lt;T, object&gt;&gt; for a generic type using the property name
    /// </summary>
    public static class ExpressionFactory
    {
        /// <summary>
        /// Creates an Expression for a property of given type using the property name
        /// </summary>
        /// <typeparam name="T">Type to create the expression for</typeparam>
        /// <param name="propertyName">Name of the property</param>
        /// <returns></returns>
        public static Expression<Func<T, object>> Create<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, propertyName);
            var target = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(target, parameter);
        }
    }
}