#pragma warning disable 1591
using System.ComponentModel;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class OptionalBooleanSearch : AbstractSearch
    {
        public OptionalBooleanSearch(string property) : base(property)
        {
        }

        public OptionalBooleanSearch()
        {
        }

        [DefaultValue(null)]
        public bool? Is { get; set; }

        public override bool IsActive() 
            => Is.HasValue;

        protected override Expression BuildFilterExpression(Expression property)
            => Is.HasValue
                   ? Expression.Equal(property, Expression.Constant(Is))
                   : null;

        protected override string DebuggerDisplay()
        {
            if (!IsActive()) return $"(OptionalBoolean) {Property}";
            return $"(OptionalBoolean) {Property} = {Is}";
        }
    }
}