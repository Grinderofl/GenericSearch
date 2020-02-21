#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Grinderofl.GenericSearch.Searches
{
    public class TextSearch : AbstractSearch
    {
        public enum Comparer
        {
            [Display(Name = "Contains")]
            Contains,

            [Display(Name = "==")]
            Equals
        }

        public TextSearch()
        {
        }

        public TextSearch(string property) : base(property)
        {
        }

        [DefaultValue(null)]
        public string Term { get; set; }

        [DefaultValue(Comparer.Equals)]
        public Comparer Is { get; set; } = Comparer.Equals;

        public override bool IsActive()
        {
            return !string.IsNullOrWhiteSpace(Term);
        }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (Term == null) return null;

            var searchExpression = Expression.Call(property,
                                                   typeof(string).GetMethod(Is.ToString(), new[] { typeof(string) }) ?? throw new InvalidOperationException(),
                                                   Expression.Constant(Term));

            return searchExpression;
        }
    }
}