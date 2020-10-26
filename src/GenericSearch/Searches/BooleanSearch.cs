#pragma warning disable 1591
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class BooleanSearch : AbstractSearch
    {
        public BooleanSearch(params string[] properties) : base(properties)
        {
        }

        [ExcludeFromCodeCoverage]
        public BooleanSearch()
        {
        }

        public bool Is { get; set; }

        public override bool IsActive() 
            => true;

        protected override Expression BuildFilterExpression(Expression property) 
            => Expression.Equal(property, Expression.Constant(Is));
    }
}