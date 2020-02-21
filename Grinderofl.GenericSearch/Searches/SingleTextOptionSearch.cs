#pragma warning disable 1591
using System.ComponentModel;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class SingleTextOptionSearch : AbstractSearch
    {
        public SingleTextOptionSearch(string property) : base(property)
        {
        }

        public SingleTextOptionSearch()
        {
        }

        [DefaultValue(null)]
        public string Is { get; set; }

        public override bool IsActive()
        {
            return !string.IsNullOrWhiteSpace(Is);
        }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (!IsActive())
            {
                return null;
            }

            return Expression.Equal(property, Expression.Constant(Is));
        }
    }
}