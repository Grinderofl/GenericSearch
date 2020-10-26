#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class SingleDateOptionSearch : AbstractSearch
    {
        public SingleDateOptionSearch(params string[] properties) : base(properties)
        {
        }

        [ExcludeFromCodeCoverage]
        public SingleDateOptionSearch()
        {
        }

        [DefaultValue(null)]
        public DateTime? Is { get; set; }

        public override bool IsActive()
        {
            return Is.HasValue;
        }

        protected override Expression BuildFilterExpression(Expression property)
        {
            Debug.Assert(Is != null, nameof(Is) + " != null");
            return Expression.Equal(property, Expression.Constant(Is.Value.Date));
        }
    }
}