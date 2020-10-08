#pragma warning disable 1591
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class TrueBooleanSearch : AbstractSearch
    {
        public TrueBooleanSearch(params string[] properties) : base(properties)
        {
        }

        [ExcludeFromCodeCoverage]
        public TrueBooleanSearch()
        {
        }

        public bool Is { get; set; }

        public override bool IsActive() => Is;

        protected override Expression BuildFilterExpression(Expression property) 
            => Expression.Equal(property, Expression.Constant(true));
    }
}