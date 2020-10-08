#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Searches
{
    [ExcludeFromCodeCoverage]
    public abstract class AbstractSearch : ISearch
    {
        protected AbstractSearch(params string[] properties)
        {
            Properties = properties;
        }

        protected AbstractSearch()
        {
        }

        [JsonIgnore]
        protected internal string[] Properties { get; set; }

        public abstract bool IsActive();

        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            if (Properties == null || !Properties.Any() || !IsActive()) return query;

            var parameter = Expression.Parameter(typeof(T), "p");
            Expression filterExpression = null;

            foreach (var property in Properties)
            {
                var parts = property.Split('.');
                var expression = BuildFilterExpressionWithNullChecks(null, parameter, null, parts);
                if (expression == null)
                {
                    continue;
                }

                if (filterExpression == null)
                {
                    filterExpression = expression;
                    continue;
                }

                filterExpression = Expression.OrElse(filterExpression, expression);
            }

            if (filterExpression == null)
            {
                return query;
            }
            
            //var parts = Property.Split('.');

            //var parameter = Expression.Parameter(typeof(T), "p");

            //var filterExpression = BuildFilterExpressionWithNullChecks(null, parameter, null, parts);

            //if (filterExpression == null) return query;

            var predicate = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
            return query.Where(predicate);
        }

        protected abstract Expression BuildFilterExpression(Expression property);

        private Expression BuildFilterExpressionWithNullChecks(Expression filterExpression, ParameterExpression parameter, Expression property,
                                                               IReadOnlyList<string> remainingPropertyParts)
        {
            property = Expression.Property(property ?? parameter, remainingPropertyParts[0]);

            BinaryExpression nullCheckExpression;
            if (remainingPropertyParts.Count == 1)
            {
                if (!property.Type.IsValueType || property.Type.IsNullableType())
                {
                    nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                    filterExpression = Combine(filterExpression, nullCheckExpression);
                }

                if (property.Type.IsNullableType()) property = Expression.Property(property, "Value");

                Expression searchExpression;
                if (property.Type.IsCollectionType())
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());
                    searchExpression = ApplySearchExpressionToCollection(parameter, property, BuildFilterExpression(parameter));
                }
                else
                {
                    searchExpression = BuildFilterExpression(property);
                }

                return searchExpression == null ? null : Combine(filterExpression, searchExpression);
            }

            nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
            filterExpression = Combine(filterExpression, nullCheckExpression);

            if (property.Type.IsCollectionType())
            {
                parameter = Expression.Parameter(property.Type.GetGenericArguments().First());
                var searchExpression = BuildFilterExpressionWithNullChecks(null, parameter, null, remainingPropertyParts.Skip(1).ToArray());

                if (searchExpression == null) return null;

                searchExpression = ApplySearchExpressionToCollection(parameter,
                                                                     property,
                                                                     searchExpression);

                return Combine(filterExpression, searchExpression);
            }

            return BuildFilterExpressionWithNullChecks(filterExpression, parameter, property, remainingPropertyParts.Skip(1).ToArray());
        }

        private static Expression ApplySearchExpressionToCollection(ParameterExpression parameter, Expression property, Expression searchExpression)
        {
            if (searchExpression == null) return null;

            var asQueryable = typeof(Queryable).GetMethods()
                                               .Where(m => m.Name == nameof(Queryable.AsQueryable))
                                               .Single(m => m.IsGenericMethod)
                                               .MakeGenericMethod(property.Type.GetGenericArguments());

            var anyMethod = typeof(Queryable).GetMethods()
                                             .Where(m => m.Name == nameof(Queryable.Any))
                                             .Single(m => m.GetParameters().Length == 2)
                                             .MakeGenericMethod(property.Type.GetGenericArguments());

            return Expression.Call(null, anyMethod, Expression.Call(null, asQueryable, property), Expression.Lambda(searchExpression, parameter));
        }

        private static Expression Combine(Expression first, Expression second)
        {
            if (first == null) return second;

            return Expression.AndAlso(first, second);
        }
    }
}