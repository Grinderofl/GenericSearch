#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class MultipleDateOptionSearch : AbstractSearch
    {
        public MultipleDateOptionSearch(params string[] properties) : base(properties)
        {
        }

        [ExcludeFromCodeCoverage]
        public MultipleDateOptionSearch()
        {
        }

        [DefaultValue(null)]
        public DateTime[] Is { get; set; }

        public override bool IsActive()
        {
            return Is != null && Is.Any();
        }

        protected override Expression BuildFilterExpression(Expression property)
        {
            Expression searchExpression = null;
            foreach (var term in Is)
            {
                var expression = Expression.Equal(property, Expression.Constant(term.Date));

                if (searchExpression == null)
                {
                    searchExpression = expression;
                    continue;
                }

                searchExpression = Expression.Or(searchExpression, expression);
            }

            return searchExpression;
        }
    }
}