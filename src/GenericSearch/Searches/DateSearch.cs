﻿#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class DateSearch : AbstractSearch
    {
        public enum Comparer
        {
            [Display(Name = "<")]
            Less,

            [Display(Name = "<=")]
            LessOrEqual,

            [Display(Name = "==")]
            Equal,

            [Display(Name = ">=")]
            GreaterOrEqual,

            [Display(Name = ">")]
            Greater,

            [Display(Name = "Range")]
            InRange
        }

        [ExcludeFromCodeCoverage]
        public DateSearch()
        {
        }

        public DateSearch(params string[] properties) : base(properties)
        {
        }

        [DefaultValue(null)]
        [DataType(DataType.Date)]
        public DateTime? Term1 { get; set; }

        [DefaultValue(null)]
        [DataType(DataType.Date)]
        public DateTime? Term2 { get; set; }

        [DefaultValue(Comparer.Equal)]
        public Comparer Is { get; set; } = Comparer.Equal;


        public override bool IsActive()
        {
            return Term1.HasValue || Term2.HasValue;
        }

        [ExcludeFromCodeCoverage]
        protected override Expression BuildFilterExpression(Expression property)
        {
            Expression searchExpression1 = null;
            Expression searchExpression2 = null;

            if (Term1.HasValue) searchExpression1 = GetFilterExpression(property);

            if (Is == Comparer.InRange && Term2.HasValue) searchExpression2 = Expression.LessThanOrEqual(property, Expression.Constant(Term2.Value));

            if (searchExpression1 == null && searchExpression2 == null) return null;

            if (searchExpression1 != null && searchExpression2 != null)
            {
                var combinedExpression = Expression.AndAlso(searchExpression1, searchExpression2);
                return combinedExpression;
            }

            return searchExpression1 ?? searchExpression2;
        }

        [ExcludeFromCodeCoverage]
        private Expression GetFilterExpression(Expression property)
        {
            switch (Is)
            {
                case Comparer.Less:
                    return Expression.LessThan(property, Expression.Constant(Term1));
                case Comparer.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(Term1));
                case Comparer.Equal:
                    return Expression.Equal(property, Expression.Constant(Term1));
                case Comparer.GreaterOrEqual:
                case Comparer.InRange:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(Term1));
                case Comparer.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(Term1));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}