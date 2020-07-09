using System.Collections.Generic;
using GenericSearch.Definition;
using GenericSearch.Definition.Expressions;

namespace GenericSearch
{
    public class ListProfile : IListDefinitionSource
    {
        /// <summary>
        /// Creates a new list definition.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public IListExpression<TRequest, TEntity, TResult> Create<TRequest, TEntity, TResult>()
        {
            var expression = new ListExpression<TRequest, TEntity, TResult>();
            definitions.Add(expression);

            return expression;
        }

        private readonly List<IListDefinition> definitions = new List<IListDefinition>();

        List<IListDefinition> IListDefinitionSource.Definitions => definitions;
    }
}