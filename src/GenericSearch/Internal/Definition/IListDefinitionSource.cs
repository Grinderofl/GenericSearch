using System.Collections.Generic;

namespace GenericSearch.Internal.Definition
{
    public interface IListDefinitionSource
    {
        List<IListDefinition> Definitions { get; }
    }
}