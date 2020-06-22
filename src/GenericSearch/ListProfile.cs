using System.Collections.Generic;
using GenericSearch.Definition;
using GenericSearch.Definition.Expressions;

namespace GenericSearch
{
    public abstract class ListProfile : IListDefinitionSource
    {
        public IListExpression<TRequest, TItem, TResult> CreateFilter<TRequest, TItem, TResult>()
        {
            var expression = new ListExpression<TRequest, TItem, TResult>();
            definitions.Add(expression);

            return expression;
        }

        private readonly List<IListDefinition> definitions = new List<IListDefinition>();

        List<IListDefinition> IListDefinitionSource.Definitions => definitions;
    }
}