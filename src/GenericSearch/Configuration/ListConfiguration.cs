using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericSearch.Configuration
{
    [DebuggerDisplay("ListConfiguration: Request = {RequestType.Name} Item = {ItemType.Name} Result = {ResultType.Name}")]
    public class ListConfiguration : IListConfiguration
    {
        public ListConfiguration(Type requestType, Type itemType, Type resultType)
        {
            RequestType = requestType;
            ItemType = itemType;
            ResultType = resultType;
        }

        public Type RequestType { get; }
        public Type ItemType { get; }
        public Type ResultType { get; }

        IEnumerable<ISearchConfiguration> IListConfiguration.SearchConfigurations => SearchConfigurations;
        public List<ISearchConfiguration> SearchConfigurations { get; } = new List<ISearchConfiguration>();
        public IPageConfiguration PageConfiguration { get; set; }
        public IRowsConfiguration RowsConfiguration { get; set; }
        public ISortColumnConfiguration SortColumnConfiguration { get; set; }
        public ISortDirectionConfiguration SortDirectionConfiguration { get; set; }
        public List<IPropertyConfiguration> PropertyConfigurations { get; } = new List<IPropertyConfiguration>();
        public IPostRedirectGetConfiguration PostRedirectGetConfiguration { get; set; }
        public ITransferValuesConfiguration TransferValuesConfiguration { get; set; }
        public IModelActivatorConfiguration ModelActivatorConfiguration { get; set; }
    }
}