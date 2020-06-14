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
            configurations.Add(expression);

            return expression;
        }

        private readonly List<IListDefinition> configurations = new List<IListDefinition>();

        List<IListDefinition> IListDefinitionSource.Definitions => configurations;
    }
}