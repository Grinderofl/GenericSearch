using System.Collections.Generic;
using GenericSearch.Definition;
using GenericSearch.Definition.Expressions;

namespace GenericSearch
{
    public abstract class ListProfile : IListDefinitionSource
    {
        /// <summary>
        /// Creates a new list definition.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public IListExpression<TRequest, TItem, TResult> Create<TRequest, TItem, TResult>()
        {
            var expression = new ListExpression<TRequest, TItem, TResult>();
            definitions.Add(expression);

            return expression;
        }

        private readonly List<IListDefinition> definitions = new List<IListDefinition>();

        List<IListDefinition> IListDefinitionSource.Definitions => definitions;
    }
}