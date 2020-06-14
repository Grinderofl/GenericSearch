using System.Collections.Generic;

namespace GenericSearch.Definition
{
    public interface IListDefinitionSource
    {
        List<IListDefinition> Definitions { get; }
    }
}