using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericSearch.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class LambdaExtensions
    {
        public static PropertyInfo GetPropertyInfo(this LambdaExpression propertyExpression)
        {
            switch (propertyExpression.Body)
            {
                case UnaryExpression unaryExpression:
                    {
                        if (unaryExpression.Operand is MemberExpression memberExpression)
                        {
                            return (PropertyInfo)memberExpression.Member;
                        }

                        throw new ArgumentException("LambdaExpression Operand is not MemberExpression", nameof(propertyExpression));
                    }
                case MemberExpression memberExpression:
                    return (PropertyInfo)memberExpression.Member;
                default:
                    throw new ArgumentException("LambdaExpression is not UnaryExpression or MemberExpression", nameof(propertyExpression));
            }
        }

        public static string GetPropertyName(this LambdaExpression propertyExpression)
        {
            return GetPropertyInfo(propertyExpression).Name;
        }

        public static void Foo<T, TProperty>(Expression<Func<T, TProperty>> expr)
        {
            MemberExpression me;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    me = ((expr.Body is UnaryExpression ue) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expr.Body as MemberExpression;
                    break;
            }

            while (me != null)
            {
                me = me.Expression as MemberExpression;
            }
        }


        public static string GetPropertyPath<TSource>(this Expression<Func<TSource, object>> path)
        {
            var propertyVisitor = new PropertyVisitor();
            propertyVisitor.Visit(path.Body);
            propertyVisitor.Path.Reverse();
            return string.Join(".", propertyVisitor.Path.Select(x => x.Name));
        }
    }
}