#pragma warning disable 1591
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class BooleanSearch : AbstractSearch
    {
        public BooleanSearch(string property) : base(property)
        {
        }

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