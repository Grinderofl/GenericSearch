#pragma warning disable 1591
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace GenericSearch.Searches
{
    public class MultipleIntegerOptionSearch : AbstractSearch
    {
        public MultipleIntegerOptionSearch(string property) : base(property)
        {
        }

        public MultipleIntegerOptionSearch()
        {
        }

        [DefaultValue(null)]
        public int[] Is { get; set; }

        public override bool IsActive()
        {
            return Is != null && Is.Any();
        }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (Is == null || !Is.Any()) return null;

            Expression searchExpression = null;
            foreach (var term in Is)
            {
                var expression = Expression.Equal(property, Expression.Constant(term));

                if (searchExpression == null)
                {
                    searchExpression = expression;
                    continue;
                }

                searchExpression = Expression.Or(searchExpression, expression);
            }

            return searchExpression;
        }

        protected override string DebuggerDisplay()
        {
            if (!IsActive()) return $"(MultipleInteger) {Property}";

            return $"(MultipleInteger) {Property}.Is = {string.Join(", ", Is.Select(x => x.ToString()))}";
        }
    }
}