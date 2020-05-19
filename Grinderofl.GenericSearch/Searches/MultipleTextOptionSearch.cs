#pragma warning disable 1591
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class MultipleTextOptionSearch : AbstractSearch
    {
        public MultipleTextOptionSearch(string property) : base(property)
        {
        }

        public MultipleTextOptionSearch()
        {
        }

        [DefaultValue(null)]
        public string[] Is { get; set; }

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
            if (!IsActive()) return $"(MultipleText) {Property}";
            return $"(MultipleText) {Property} = {string.Join(", ", Is)}";
        }
    }
}