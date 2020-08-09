using System;
using System.Collections.Generic;
using System.Reflection;

namespace GenericSearch.Internal.Definition
{
    public interface IListDefinition
    {
        Type RequestType { get; }
        Type ItemType { get; }
        Type ResultType { get; }

        PropertyInfo[] RequestProperties { get; }
        PropertyInfo[] ResultProperties { get; }

        Dictionary<PropertyInfo, ISearchDefinition> SearchDefinitions { get; }
        
        IPageDefinition PageDefinition { get; }
        IRowsDefinition RowsDefinition { get; }
        ISortColumnDefinition SortColumnDefinition { get; }
        ISortDirectionDefinition SortDirectionDefinition { get; }

        Dictionary<PropertyInfo, IPropertyDefinition> PropertyDefinitions { get; }

        IPostRedirectGetDefinition PostRedirectGetDefinition { get; }
        ITransferValuesDefinition TransferValuesDefinition { get; }
        IModelActivatorDefinition ModelActivatorDefinition { get; }
    }
}