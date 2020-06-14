using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Internal
{
    [ExcludeFromCodeCoverage]
    internal static class ExpressionFactory
    {
        public static Expression<Func<T, object>> Create<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, propertyName);
            var target = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(target, parameter);
        }
    }
}