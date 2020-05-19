#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class SingleDateOptionSearch : AbstractSearch
    {
        public SingleDateOptionSearch(string property) : base(property)
        {
        }

        public SingleDateOptionSearch()
        {
        }

        [DefaultValue(null)]
        public DateTime? Is { get; set; }

        public override bool IsActive()
        {
            return Is.HasValue;
        }

        protected override Expression BuildFilterExpression(Expression property)
            => Is.HasValue
                   ? Expression.Equal(property, Expression.Constant(Is.Value.Date))
                   : null;

        protected override string DebuggerDisplay()
        {
            if (!IsActive()) return $"(SingleDateOption) {Property}";
            return $"(SingleDateOption) {Property}.Is = {Is}";
        }
    }
}