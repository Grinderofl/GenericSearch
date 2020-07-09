#pragma warning disable 1591
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class OptionalBooleanSearch : AbstractSearch
    {
        public OptionalBooleanSearch(string property) : base(property)
        {
        }

        [ExcludeFromCodeCoverage]
        public OptionalBooleanSearch()
        {
        }

        [DefaultValue(null)]
        public bool? Is { get; set; }

        public override bool IsActive() 
            => Is.HasValue;

        protected override Expression BuildFilterExpression(Expression property) 
            => Expression.Equal(property, Expression.Constant(Is));
    }
}