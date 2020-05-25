using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grinderofl.GenericSearch.Internal.Extensions
{
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
    }
}