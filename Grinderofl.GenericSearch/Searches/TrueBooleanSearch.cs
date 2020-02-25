﻿#pragma warning disable 1591
using System.ComponentModel;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class TrueBooleanSearch : AbstractSearch
    {
        public TrueBooleanSearch(string property) : base(property)
        {
        }

        public TrueBooleanSearch()
        {
        }

        public bool Is { get; set; }

        public override bool IsActive() => Is;

        protected override Expression BuildFilterExpression(Expression property)
            => Is
                   ? Expression.Equal(property, Expression.Constant(true))
                   : null;
    }
}