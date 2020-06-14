#pragma warning disable 1591
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericSearch.Searches
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

        [ExcludeFromCodeCoverage]
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

        private static readonly MethodInfo Contains =
            typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)});

        protected override Expression BuildFilterExpression(Expression property)
        {
            var constant = Expression.Constant(Term);

            if (Is == Comparer.Contains)
            {
                return Expression.Call(property, Contains, constant);
            }
                
            return Expression.Equal(property, constant);
        }
    }
}