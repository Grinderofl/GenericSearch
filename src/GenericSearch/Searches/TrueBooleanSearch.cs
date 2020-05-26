#pragma warning disable 1591
using System.Linq.Expressions;

namespace GenericSearch.Searches
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

        protected override string DebuggerDisplay()
        {
            if (!IsActive()) return $"(TrueBoolean) {Property}";
            return $"(TrueBoolean) {Property}.Is = {Is}";
        }
    }
}