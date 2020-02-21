﻿#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class MultipleDateOptionSearch : AbstractSearch
    {
        public MultipleDateOptionSearch(string property) : base(property)
        {
        }

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
            if (Is == null || !Is.Any()) return null;

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