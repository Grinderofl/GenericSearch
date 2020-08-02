using System;
using System.Collections.Generic;

namespace GenericSearch.Configuration
{
    public interface IListConfiguration
    {
        Type RequestType { get; }
        Type ItemType { get; }
        Type ResultType { get; }
        IEnumerable<ISearchConfiguration> SearchConfigurations { get; }
        IPageConfiguration PageConfiguration { get; }
        IRowsConfiguration RowsConfiguration { get; }
        ISortColumnConfiguration SortColumnConfiguration { get; }
        ISortDirectionConfiguration SortDirectionConfiguration { get; }
        List<IPropertyConfiguration> PropertyConfigurations { get; }
        IPostRedirectGetConfiguration PostRedirectGetConfiguration { get; }
        ITransferValuesConfiguration TransferValuesConfiguration { get; }
        IModelActivatorConfiguration ModelActivatorConfiguration { get; }
    }
}