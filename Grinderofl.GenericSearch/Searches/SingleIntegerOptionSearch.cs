#pragma warning disable 1591
using System.ComponentModel;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class SingleIntegerOptionSearch : AbstractSearch
    {
        public SingleIntegerOptionSearch(string property) : base(property)
        {
        }

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