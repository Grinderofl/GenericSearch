﻿#pragma warning disable 1591
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class SingleIntegerOptionSearch : AbstractSearch
    {
        public SingleIntegerOptionSearch(params string[] properties) : base(properties)
        {
        }

        [ExcludeFromCodeCoverage]
        public SingleIntegerOptionSearch()
        {
        }

        [DefaultValue(null)]
        public int? Is { get; set; }

        public override bool IsActive()
        {
            return Is.HasValue;
        }

        protected override Expression BuildFilterExpression(Expression property)
            => Is.HasValue
                   ? Expression.Equal(property, Expression.Constant(Is))
                   : null;
    }
}